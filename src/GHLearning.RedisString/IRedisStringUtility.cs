using StackExchange.Redis;

namespace GHLearning.RedisString;

public interface IRedisStringUtility
{
	Task StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<RedisValue> StringGetAsync(RedisKey key, CancellationToken cancellationToken = default);

	Task KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<bool> KeyExistsAsync(RedisKey key, CancellationToken cancellationToken = default);
}
