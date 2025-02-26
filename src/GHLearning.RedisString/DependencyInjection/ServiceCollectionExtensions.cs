using GHLearning.RedisString;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRedisString(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services.AddTransient<IRedisStringUtility>((sp) => ActivatorUtilities.CreateInstance<RedisStringUtility>(sp, redisDatabaseResolver(sp)));
}
