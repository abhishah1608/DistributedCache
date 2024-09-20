using DistributedCache.Helper;
using DistributedCache.Service;
using Microsoft.AspNetCore.HttpLogging;
using StackExchange.Redis;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string configPath = Directory.GetCurrentDirectory() + "\\" + "Config";

string envfile = builder.Environment.EnvironmentName == "Development" ? "dev" : builder.Environment.EnvironmentName == "Staging" ? "stag" : "prod";

// set configuration path.
builder.Configuration
    .SetBasePath(configPath) // Optional, if needed for path resolution
    .AddJsonFile($"appsettings-{envfile}.json", optional: true, reloadOnChange: true) // Environment-specific settings
    .AddEnvironmentVariables(); // Optional: if you're using environment variables


// database connection.

builder.Services.AddScoped<SqlConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));




// Redis Cache.
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection"), true); // Replace with your Redis server address
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<RedisCacheService>();

builder.Services.AddControllers();

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

app.Run();
