using GHLearning.RedisSortedSet;

using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddRedisSortedSet(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services.AddTransient<IRedisSortedSetUtility>((sp) => ActivatorUtilities.CreateInstance<RedisSortedSetUtility>(sp, redisDatabaseResolver(sp)));
}
