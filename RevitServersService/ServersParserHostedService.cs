using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RevitServerParser.Models;
using RevitServerParser.Parser;
using RevitServersService;
using RevitServersService.db;

internal class ServersParserHostedService : BackgroundService
{
    private readonly ILogger<ServersParserHostedService> _logger;
    private readonly IOptionsMonitor<List<RevitServerOpt>> _servers;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ParseResultService parseResultService;
    private readonly IServiceProvider serviceProvider;
    private readonly HttpClient client;

    public bool InProcess { get; private set; }
    public DateTime LastParseTime { get; private set; }
    public ServersParserHostedService(ILogger<ServersParserHostedService> logger,
        IOptionsMonitor<List<RevitServerOpt>> servers,
        IHttpClientFactory httpClientFactory,
        ParseResultService parseResultService,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _servers = servers;
        this.httpClientFactory = httpClientFactory;
        this.parseResultService = parseResultService;
        this.serviceProvider = serviceProvider;
        client = httpClientFactory.CreateClient("def");

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ServersParserHostedService running.");

        try
        {
            await TryAddServers(stoppingToken);

            await DoWork(stoppingToken);
#if DEBUG
            using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
#else
            using PeriodicTimer timer = new(TimeSpan.FromMinutes(10));
#endif
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

    private async Task TryAddServers(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ServersDbContext>();

        await context.Database.EnsureCreatedAsync(stoppingToken);

        if (await context.RevitServers.AnyAsync(stoppingToken))
            return;

        var servers = CreateTempServers().Select(x => new RSToParse() { Host = x.host, Year = x.year });

        context.RevitServers.AddRange(servers);

        await context.SaveChangesAsync(stoppingToken);

    }

    record tempServer(string host, int year);
    private async Task DoWork(CancellationToken stoppingToken)
    {
        InProcess = true;
        _logger.LogInformation("Start Work");

        var results = await Parse(stoppingToken);

        LastParseTime = DateTime.UtcNow;

        parseResultService.Add(new ParseResult(LastParseTime, results!));

        _logger.LogInformation("End Work");
        InProcess = false;
    }

    private async Task<List<RevitServer>> Parse(CancellationToken stoppingToken)
    {
        List<RSToParse>? servers = null;

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ServersDbContext>();
            servers = await context.RevitServers.ToListAsync(stoppingToken);
        }

        if (servers == null)
            return Enumerable.Empty<RevitServer>().ToList();

        var parsers = servers.Select(s => new ServerParser(s.Host, s.Year, client)).ToList();

        var tasks = parsers.Select(x => x.ParseServer(3, stoppingToken)).ToList();

        var gt = Task.WhenAll(tasks);

        try
        {
            await gt;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.ToString());
        }

        if (gt.Status != TaskStatus.RanToCompletion)
            return Enumerable.Empty<RevitServer>().ToList();

        var results = tasks.Select(t => t.Result)
            .Where(x => x is not null && x.Folders?.Count > 0)
            .ToList();
        return results!;
    }


    private List<tempServer> CreateTempServers()
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