using GHLearning.RedisHash;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRedisHash(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services.AddTransient<IRedisHashUtility>((sp) => ActivatorUtilities.CreateInstance<RedisHashUtility>(sp, redisDatabaseResolver(sp)));
}
