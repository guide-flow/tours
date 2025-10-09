using Core.UseCases;
using DotNetEnv;
using Infrastructure;
using NATS.Client;
using Tour.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureTours();

builder.Services.AddSingleton<IConnection>(sp => 
{
    var config = sp.GetRequiredService<IConfiguration>();
    var url = config.GetValue<string>("NATS_URL") ?? "nats://localhost:4222";
    var cf = new ConnectionFactory();
    return cf.CreateConnection(url);
});

builder.Services.AddSingleton<PurchaseSagaHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var purchaseSagaHandler = app.Services.GetRequiredService<PurchaseSagaHandler>();
purchaseSagaHandler.Subscribe();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();
