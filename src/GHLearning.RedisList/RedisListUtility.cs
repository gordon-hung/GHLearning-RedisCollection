using StackExchange.Redis;

namespace GHLearning.RedisList;

internal class RedisListUtility(IDatabase datebase) : IRedisListUtility
{
	public Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.ListRightPushAsync(key, value, when, flags);

	public Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.ListRemoveAsync(key, value, count, flags);

	public Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.ListRangeAsync(key, start, stop, flags);

	public Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.ListSetByIndexAsync(key, index, value, flags);
}
