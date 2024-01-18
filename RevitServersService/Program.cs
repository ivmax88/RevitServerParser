using System.Text.Json.Serialization;
using RevitServersService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
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

app.UseFileServer();

app.MapControllers();

app.Run();



