using StackExchange.Redis;

namespace GHLearning.RedisSortedSet;

internal class RedisSortedSetUtility(IDatabase datebase) : IRedisSortedSetUtility
{
	public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SortedSetAddAsync(key, member, score, when, flags);

	public Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SortedSetRemoveAsync(key, value, flags);

	public Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SortedSetRangeByRankAsync(key, start, stop, order, flags);

	public Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SortedSetScoreAsync(key, member, flags);
}
