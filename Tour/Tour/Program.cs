using Core.UseCases;
using DotNetEnv;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Tour.ProtoControllers;
using NATS.Client;
using Prometheus;
using Tour.Startup;

// Enable HTTP/2 without TLS for gRPC (required for non-HTTPS gRPC)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services
    .AddGrpc()
    .AddJsonTranscoding();               

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

var certPath = Environment.GetEnvironmentVariable("Kestrel_Cert_Path");
var certPassword = Environment.GetEnvironmentVariable("Kestrel_Cert_Password");
var httpPort = int.TryParse(Environment.GetEnvironmentVariable("Kestrel_Http_Port"), out var hp) ? hp : 5229;
var httpsPort = int.TryParse(Environment.GetEnvironmentVariable("Kestrel_Https_Port"), out var sp) ? sp : 7029;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(httpPort, listen =>
    {
        listen.Protocols = HttpProtocols.Http1AndHttp2;
    });

    // Only configure HTTPS if certificate path is provided
    if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(certPassword))
    {
        options.ListenAnyIP(httpsPort, listen =>
        {
            listen.UseHttps(certPath, certPassword);
            listen.Protocols = HttpProtocols.Http1AndHttp2;
        });
    }
});

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

//app.UseHttpsRedirection();

app.UseCors("_allowDevClients");

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<TourProtoController>();

app.MapGrpcService<ShoppingCartProtoController>();

app.ApplyMigrations();

app.UseHttpMetrics();
app.MapMetrics();

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();
