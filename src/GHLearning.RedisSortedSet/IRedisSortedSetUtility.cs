using StackExchange.Redis;

namespace GHLearning.RedisSortedSet;

public interface IRedisSortedSetUtility
{
	Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
	Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
	Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
	Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
}
