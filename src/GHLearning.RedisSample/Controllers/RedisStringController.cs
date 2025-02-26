using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using GHLearning.RedisSample.Models;
using GHLearning.RedisSample.ViewModels;
using GHLearning.RedisString;

namespace GHLearning.RedisSample.Controllers;

[Route("api/Redis-String")]
[ApiController]
public class RedisStringController : ControllerBase
{
	[HttpGet("{account}")]
	public async Task<UserViewModel?> GetAsync(
		[FromServices] IRedisStringUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisStringController).Name, ":", account);
		var value = await utility.StringGetAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		UserModel? user = default;

		if (!string.IsNullOrEmpty(value))
		{
			user = JsonSerializer.Deserialize<UserModel?>(value!);
		}

		return user is null
			? null
			: new UserViewModel(
				Account: user.Account,
				Name: user.Name);
	}

	[HttpPost()]
	public async Task AddAsync(
		[FromServices] IRedisStringUtility utility,
		[FromBody] UserAddViewModel source)
	{
		var key = string.Concat(typeof(RedisStringController).Name, ":", source.Account);

		if (await utility.KeyExistsAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false))
		{
			throw new ArgumentException(source.Account);
		}

		await utility.StringSetAsync(
			key: key,
			value: JsonSerializer.Serialize(new UserModel
			{
				Account = source.Account,
				Name = source.Name
			}),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpDelete("{account}")]
	public async Task DeleteAsync(
		[FromServices] IRedisStringUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisStringController).Name, ":", account);

		await utility.KeyDeleteAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpPatch("{account}")]
	public async Task UpdateAsync(
		[FromServices] IRedisStringUtility utility,
		string account,
		[FromBody] UserUpdateViewModel source)
	{
		var key = string.Concat(typeof(RedisStringController).Name, ":", account);

		if (!await utility.KeyExistsAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false))
		{
			throw new ArgumentException(account);
		}

		var value = await utility.StringGetAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		var user = JsonSerializer.Deserialize<UserModel?>(value!);

		ArgumentNullException.ThrowIfNull(user, nameof(user));

		user.Name = source.Name;

		await utility.StringSetAsync(
			key: key,
			value: JsonSerializer.Serialize(user),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}
}
