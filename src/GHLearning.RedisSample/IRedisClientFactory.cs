using StackExchange.Redis;

namespace GHLearning.RedisSample;

public interface IRedisClientFactory
{
	IDatabase GetDatabase();

	ConnectionMultiplexer GetConnectionMultiplexer();
}
