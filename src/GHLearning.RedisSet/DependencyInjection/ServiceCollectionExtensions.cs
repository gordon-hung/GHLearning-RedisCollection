using GHLearning.RedisSet;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRedisSet(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services.AddTransient<IRedisSetUtility>((sp) => ActivatorUtilities.CreateInstance<RedisSetUtility>(sp, redisDatabaseResolver(sp)));
}
