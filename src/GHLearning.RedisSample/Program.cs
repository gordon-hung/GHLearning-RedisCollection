using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using GHLearning.RedisSample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
	.AddRouting(options => options.LowercaseUrls = true)
	.AddControllers(options =>
	{
		options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
		options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
	})
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptions<RedisClientOptions>()
	.Configure((RedisClientOptions op) => op.ConnectionString = builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddSingleton<IRedisClientFactory, RedisClientFactory>()
	.AddRedisString(redisDatabaseResolver: (IServiceProvider sp) => sp.GetRequiredService<IRedisClientFactory>().GetDatabase())
	.AddRedisHash(redisDatabaseResolver: (IServiceProvider sp) => sp.GetRequiredService<IRedisClientFactory>().GetDatabase())
	.AddRedisList(redisDatabaseResolver: (IServiceProvider sp) => sp.GetRequiredService<IRedisClientFactory>().GetDatabase())
	.AddRedisSet(redisDatabaseResolver: (IServiceProvider sp) => sp.GetRequiredService<IRedisClientFactory>().GetDatabase())
	.AddRedisSortedSet(redisDatabaseResolver: (IServiceProvider sp) => sp.GetRequiredService<IRedisClientFactory>().GetDatabase());

builder.Services
	.AddHealthChecks()
	.AddCheck("self", () => HealthCheckResult.Healthy(), ["live"])
	.AddRedis(
	(sp) => sp.GetRequiredService<IRedisClientFactory>().GetConnectionMultiplexer(),
	name: "Redis");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));// swagger/
	app.UseReDoc(options => options.SpecUrl("/openapi/v1.json"));//api-docs/
	app.MapScalarApiReference();//scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	Predicate = check => check.Tags.Contains("live"),
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});
app.UseHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	Predicate = _ => true,
	ResultStatusCodes =
	{
		[HealthStatus.Healthy] = StatusCodes.Status200OK,
		[HealthStatus.Degraded] = StatusCodes.Status200OK,
		[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
	}
});

app.Run();
