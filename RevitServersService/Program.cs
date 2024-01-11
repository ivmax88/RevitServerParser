using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RevitServersService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddOptions<List<RevitServerOpt>>()
    .Bind(builder.Configuration.GetSection("RevitServers"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<ParseResultService>();
builder.Services.AddHostedService<ServersParserHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


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


app.MapGet("/get/{year}", (int year, ParseResultService service) =>
{
    return service.Get(year);
})
.WithName("GetByYear")
.WithOpenApi();


app.MapGet("/get/{host}", (string host, ParseResultService service) =>
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


app.Run();