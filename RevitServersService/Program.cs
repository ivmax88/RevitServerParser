using Microsoft.Extensions.Options;
using RevitServersService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<List<RevitServer>>()
    .Bind(builder.Configuration.GetSection("RevitServers"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/test", (IOptionsMonitor<List<RevitServer>> options) =>
{
    return options.CurrentValue;
})
.WithName("Test")
.WithOpenApi();

app.Run();