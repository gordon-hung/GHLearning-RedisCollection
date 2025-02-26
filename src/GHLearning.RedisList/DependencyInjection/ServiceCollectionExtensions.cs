using GHLearning.RedisList;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRedisList(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services.AddTransient<IRedisListUtility>((sp) => ActivatorUtilities.CreateInstance<RedisListUtility>(sp, redisDatabaseResolver(sp)));
}
