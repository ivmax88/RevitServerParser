using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using RevitServersService;
using RevitServersService.db;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ServersDbContext>(opt
    => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

});
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("def", httpClient =>
{
#if RELEASE
    httpClient.Timeout = TimeSpan.FromMinutes(3);
#endif
});

builder.Services.ConfigureHttpClientDefaults(b =>
{
    b.RemoveAllLoggers();
});


builder.Services.AddOptions<List<RevitServerOpt>>()
    .Bind(builder.Configuration.GetSection("RevitServers"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<ParseResultService>();
builder.Services.AddHostedService<ServersParserHostedService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", builder =>
     builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("NewPolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.UseFileServer();

app.MapControllers();

app.Run();
