using StackExchange.Redis;

namespace GHLearning.RedisHash;

internal class RedisHashUtility(IDatabase datebase) : IRedisHashUtility
{
	public Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.HashSetAsync(key, hashFields, flags);

	public Task KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.KeyDeleteAsync(key, flags);

	public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default)
		=> datebase.HashGetAllAsync(key, flags);

	public Task<bool> KeyExistsAsync(RedisKey key, CancellationToken cancellationToken = default)
		=> datebase.KeyExistsAsync(key);
}
