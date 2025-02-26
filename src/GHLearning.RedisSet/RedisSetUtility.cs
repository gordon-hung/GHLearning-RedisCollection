using StackExchange.Redis;

namespace GHLearning.RedisSet;

internal class RedisSetUtility(IDatabase datebase) : IRedisSetUtility
{
	public Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SetAddAsync(key, value, flags);

	public Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SetRemoveAsync(key, value, flags);

	public Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.SetMembersAsync(key, flags);
}
