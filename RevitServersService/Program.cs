using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using RevitServersService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient().ConfigureHttpClientDefaults(b =>
{
    b.RemoveAllLoggers();
});

builder.Services.AddOptions<List<RevitServerOpt>>()
    .Bind(builder.Configuration.GetSection("RevitServers"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<ParseResultService>();
builder.Services.AddHostedService<ServersParserHostedService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

api(app);

app.Run();



static void api(WebApplication app)
{
    app.MapGet("/test", (IOptionsMonitor<List<RevitServerOpt>> options) =>
    {
        return options.CurrentValue;
    })
    .WithName("Test")
    .WithOpenApi();


    app.MapGet("/getall", (ParseResultService service) =>
    {
        return service.GetAll();
    })
    .WithName("GetAll")
    .WithOpenApi();


    app.MapGet("/get-year/{year}", (int year, ParseResultService service) =>
    {
        return service.Get(year);
    })
    .WithName("GetByYear")
    .WithOpenApi();


    app.MapGet("/get-host/{host}", (string host, ParseResultService service) =>
    {
        return service.Get(host);
    })
    .WithName("GetByHost")
    .WithOpenApi();


    app.MapGet("/getProjects/{name}", (string name, ParseResultService service) =>
    {
        return service.GetProjectByName(name);
    })
    .WithName("GetProjectByName")
    .WithOpenApi();


    app.MapGet("/getFolders/{name}", (string name, ParseResultService service) =>
    {
        return service.GetAllFoldersByName(name);
    })
    .WithName("GetAllFoldersByName")
    .WithOpenApi();


    app.MapGet("/getModels/{name}", (string name, ParseResultService service) =>
    {
        return service.GetAllModelsByName(name);
    })
    .WithName("GetAllModelsByName")
    .WithOpenApi();


    app.MapGet("/getModelHistory/{host}/{year}/{path}", async (string host, int year, string path,
        ParseResultService service, CancellationToken token) =>
    {
        try
        {
            return Results.Ok(await service.GetModelHistory(host, year, path, token));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    })
    .WithName("GetModelHistory")
    .WithOpenApi();


    app.MapGet("/getModelInfo/{host}/{year}/{path}", async (string host, int year, string path,
        ParseResultService service, CancellationToken token) =>
    {
        try
        {
            return Results.Ok(await service.GetModelInfo(host, year, path, token));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    })
    .WithName("GetModelInfo")
    .WithOpenApi();
}