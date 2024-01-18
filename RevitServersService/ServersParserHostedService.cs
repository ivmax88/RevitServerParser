using System.Text.Json;
using Microsoft.Extensions.Options;
using RevitServerParser;
using RevitServerParser.Parser;
using RevitServerParser.RevitServerModels;
using RevitServersService;

internal class ServersParserHostedService : BackgroundService
{
    private readonly ILogger<ServersParserHostedService> _logger;
    private readonly IOptionsMonitor<List<RevitServerOpt>> _servers;
    private readonly HttpClient _client;
    private readonly ParseResultService parseResultService;

    public bool InProcess { get; private set; }
    public DateTime LastParseTime { get; private set; }
    public ServersParserHostedService(ILogger<ServersParserHostedService> logger,
        IOptionsMonitor<List<RevitServerOpt>> servers,
        HttpClient client,
        ParseResultService parseResultService)
    {
        _logger = logger;
        _servers = servers;
        _client = client;
        this.parseResultService = parseResultService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ServersParserHostedService running.");

        await DoWork(stoppingToken);
#if DEBUG
        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
#else
        using PeriodicTimer timer = new(TimeSpan.FromMinutes(10));
#endif

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    record tempServer(string host, int year);
    private async Task DoWork(CancellationToken stoppingToken)
    {
        InProcess = true;
        _logger.LogInformation("Start Work");

        if (_servers == null) return;

        var results = await Parse(stoppingToken);

        LastParseTime = DateTime.UtcNow;

        parseResultService.Add(new ParseResult(LastParseTime, results!));

        _logger.LogInformation("End Work");
        InProcess = false;
    }

    private async Task<List<RevitServerParser.Models.RevitServer>> Parse(CancellationToken stoppingToken)
    {
        var tempServers = CreateTempSerers();

        var parsers = tempServers.Select(s => new ServerParser(s.host, s.year, _client)).ToList();

        var tasks = parsers.Select(x => x.ParseServer(3, stoppingToken)).ToList();

        await Task.WhenAll(tasks);

        var results = tasks.Select(t => t.Result)
            .Where(x => x is not null && x.Folders?.Count > 0)
            .ToList();
        return results!;
    }


    private List<tempServer> CreateTempSerers()
    {
        var result = new List<tempServer>();

#if DEBUG
        foreach (var server in _servers.CurrentValue.Skip(2))
            foreach (var host in server.Hosts)
                result.Add(new tempServer(host, server.Year));
#else
        foreach (var server in _servers.CurrentValue)
                    foreach (var host in server.Hosts)
                        result.Add(new tempServer(host, server.Year));
#endif
        return result;
    }
}