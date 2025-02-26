using StackExchange.Redis;

namespace GHLearning.RedisHash;

public interface IRedisHashUtility
{
	Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None, CancellationToken cancellationToken = default);

	Task<bool> KeyExistsAsync(RedisKey key, CancellationToken cancellationToken = default);
}
