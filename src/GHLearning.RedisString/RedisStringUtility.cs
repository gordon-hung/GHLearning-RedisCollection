using StackExchange.Redis;

namespace GHLearning.RedisString;

internal class RedisStringUtility(
	IDatabase database) : IRedisStringUtility
{
	public Task KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> database.KeyDeleteAsync(key, flags);

	public Task StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> database.StringSetAsync(key, value, expiry, keepTtl, when, flags);

	public Task<RedisValue> StringGetAsync(RedisKey key, CancellationToken cancellationToken = default)
		=> database.StringGetAsync(key);

	public Task<bool> KeyExistsAsync(RedisKey key, CancellationToken cancellationToken = default)
		=> database.KeyExistsAsync(key);
}
