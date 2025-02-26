using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GHLearning.RedisSample;

internal class RedisClientFactory : IRedisClientFactory, IDisposable
{
	private readonly ConnectionMultiplexer _connectionMultiplexer;
	private bool _disposedValue;

	public RedisClientFactory(IOptions<RedisClientOptions> optionsAccessor)
	{
		var options = optionsAccessor.Value;
		_connectionMultiplexer = ConnectionMultiplexer.Connect(options.ConnectionString);
	}

	public void Dispose()
	{
		// 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public IDatabase GetDatabase()
	{
		return _connectionMultiplexer.GetDatabase();
	}

	public ConnectionMultiplexer GetConnectionMultiplexer()
	{
		return _connectionMultiplexer;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_connectionMultiplexer.Close();
				_connectionMultiplexer.Dispose();
			}

			_disposedValue = true;
		}
	}
}
