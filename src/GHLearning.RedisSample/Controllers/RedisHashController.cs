using GHLearning.RedisHash;
using GHLearning.RedisSample.ViewModels;

using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GHLearning.RedisSample.Controllers;

[Route("api/Redis-Hash")]
[ApiController]
public class RedisHashController : ControllerBase
{
	[HttpGet("{account}")]
	public async Task<UserViewModel?> GetAsync(
		[FromServices] IRedisHashUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisHashController).Name, ":", account);
		var hashEntries = await utility.HashGetAllAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (hashEntries.Length == 0)
		{
			return null; // 如果找不到該用戶，則返回 null
		}

		var userData = hashEntries.ToDictionary(x => x.Name, x => x.Value.ToString());

		return new UserViewModel(
			Account: userData.GetValueOrDefault("Account")!,
			Name: userData.GetValueOrDefault("Name")!);
	}

	[HttpPost()]
	public async Task AddAsync(
		[FromServices] IRedisHashUtility utility,
		[FromBody] UserAddViewModel source)
	{
		var key = string.Concat(typeof(RedisHashController).Name, ":", source.Account);

		if (await utility.KeyExistsAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false))
		{
			throw new ArgumentException(source.Account);
		}

		await utility.HashSetAsync(key,
			[
			new HashEntry("Account", source.Account),
			new HashEntry("Name", source.Name)
			],
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpDelete("{account}")]
	public async Task DeleteAsync(
		[FromServices] IRedisHashUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisHashController).Name, ":", account);

		await utility.KeyDeleteAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpPatch("{account}")]
	public async Task UpdateAsync(
		[FromServices] IRedisHashUtility utility,
		string account,
		[FromBody] UserUpdateViewModel source)
	{
		var key = string.Concat(typeof(RedisHashController).Name, ":", account);

		if (!await utility.KeyExistsAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false))
		{
			throw new ArgumentException(account);
		}

		await utility.HashSetAsync(key,
			[new HashEntry("Name", source.Name)],
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}
}
