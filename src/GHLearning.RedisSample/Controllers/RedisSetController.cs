using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using GHLearning.RedisSample.Models;
using GHLearning.RedisSample.ViewModels;
using GHLearning.RedisSet;

namespace GHLearning.RedisSample.Controllers;

[Route("api/Redis-Set")]
[ApiController]
public class RedisSetController : ControllerBase
{
	[HttpGet("{account}")]
	public async Task<UserViewModel?> GetAsync(
		[FromServices] IRedisSetUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisSetController).Name);
		var redisValues = await utility.SetMembersAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (redisValues.Length == 0)
		{
			return null; // 如果找不到該用戶，則返回 null
		}

		UserModel? user = default;
		if (redisValues.Length > 0)
		{
			foreach (var value in redisValues.Select((redisValue, index) => new
			{
				RedisValue = redisValue,
				Index = index
			}))
			{
				user = JsonSerializer.Deserialize<UserModel>(value.RedisValue!.ToString()!);

				if (user is null)
				{
					continue;
				}

				if (user.Account.Equals(account, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
			}
		}

		return user is null
			? null
			: new UserViewModel(
				Account: user.Account,
				Name: user.Name);
	}

	[HttpPost()]
	public async Task AddAsync(
		[FromServices] IRedisSetUtility utility,
		[FromBody] UserAddViewModel source)
	{
		var key = string.Concat(typeof(RedisSetController).Name);

		var redisValues = await utility.SetMembersAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (redisValues.Length > 0)
		{
			foreach (var value in redisValues.Select((redisValue, index) => new
			{
				RedisValue = redisValue,
				Index = index
			}))
			{
				var user = JsonSerializer.Deserialize<UserModel>(value.RedisValue!.ToString()!);

				if (user is null)
				{
					continue;
				}

				if (user.Account.Equals(source.Account, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(source.Account);
				}
			}
		}

		await utility.SetAddAsync(key,
			JsonSerializer.Serialize(new UserModel
			{
				Account = source.Account,
				Name = source.Name
			}),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpDelete("{account}")]
	public async Task DeleteAsync(
		[FromServices] IRedisSetUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisSetController).Name);

		var redisValues = await utility.SetMembersAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		if (redisValues.Length > 0)
		{
			foreach (var redisValue in redisValues)
			{
				var user = JsonSerializer.Deserialize<UserModel>(redisValue!.ToString()!);

				if (user is null)
				{
					continue;
				}

				if (user.Account.Equals(account, StringComparison.OrdinalIgnoreCase))
				{
					await utility.SetRemoveAsync(key, redisValue).ConfigureAwait(false);
					break;
				}
			}
		}
	}

	[HttpPatch("{account}")]
	public async Task UpdateAsync(
		[FromServices] IRedisSetUtility utility,
		string account,
		[FromBody] UserUpdateViewModel source)
	{
		var key = string.Concat(typeof(RedisSetController).Name);

		var redisValues = await utility.SetMembersAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		UserModel? user = default;
		if (redisValues.Length > 0)
		{
			foreach (var redisValue in redisValues)
			{
				user = JsonSerializer.Deserialize<UserModel>(redisValue.ToString()!);

				if (user is null)
				{
					continue;
				}

				if (user.Account.Equals(account, StringComparison.OrdinalIgnoreCase))
				{
					await utility.SetRemoveAsync(key, redisValue, cancellationToken: HttpContext.RequestAborted).ConfigureAwait(false);
					break;
				}
			}
		}

		if (user is null)
		{
			throw new ArgumentException(account);
		}

		user.Name = source.Name;

		await utility.SetAddAsync(
			key,
			JsonSerializer.Serialize(user),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}
}
