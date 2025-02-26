using StackExchange.Redis;

namespace GHLearning.RedisList;

public interface IRedisListUtility
{
	public Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	public Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	public Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	public Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
}
