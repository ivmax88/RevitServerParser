using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RevitServersService;
using RevitServersService.db;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ServersDbContext>(opt
    => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(o=>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

});
//builder.Services.ConfigureHttpJsonOptions(opt =>
//{
//    opt.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

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
