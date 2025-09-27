using DotNetEnv;
using Infrastructure;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();
