namespace GHLearning.RedisSample;

public record RedisClientOptions
{
	public string ConnectionString { get; set; } = "localhost";
}
