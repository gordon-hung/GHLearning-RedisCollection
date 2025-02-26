using StackExchange.Redis;

namespace GHLearning.RedisSet;

public interface IRedisSetUtility
{
	Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);
}
