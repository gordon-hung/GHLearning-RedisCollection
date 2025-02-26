using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using GHLearning.RedisSample.Models;
using GHLearning.RedisSample.ViewModels;
using GHLearning.RedisSortedSet;

namespace GHLearning.RedisSample.Controllers;

[Route("api/Redis-SortedSet")]
[ApiController]
public class RedisSortedSetController : ControllerBase
{
	[HttpGet("{account}")]
	public async Task<UserViewModel?> GetAsync(
		[FromServices] IRedisSortedSetUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisSortedSetController).Name);
		var redisValues = await utility.SortedSetRangeByRankAsync(
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
		[FromServices] IRedisSortedSetUtility utility,
		[FromBody] UserAddViewModel source)
	{
		var key = string.Concat(typeof(RedisSortedSetController).Name);

		var redisValues = await utility.SortedSetRangeByRankAsync(
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

		await utility.SortedSetAddAsync(key,
			JsonSerializer.Serialize(new UserModel
			{
				Account = source.Account,
				Name = source.Name
			}),
			redisValues.Length + 1,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	[HttpDelete("{account}")]
	public async Task DeleteAsync(
		[FromServices] IRedisSortedSetUtility utility,
		string account)
	{
		var key = string.Concat(typeof(RedisSortedSetController).Name);

		var redisValues = await utility.SortedSetRangeByRankAsync(
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
					await utility.SortedSetRemoveAsync(key, redisValue).ConfigureAwait(false);
					break;
				}
			}
		}
	}

	[HttpPatch("{account}")]
	public async Task UpdateAsync(
		[FromServices] IRedisSortedSetUtility utility,
		string account,
		[FromBody] UserUpdateViewModel source)
	{
		var key = string.Concat(typeof(RedisSortedSetController).Name);

		var redisValues = await utility.SortedSetRangeByRankAsync(
			key: key,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		UserModel? user = default;
		double? score = default;
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
					score = await utility.SortedSetScoreAsync(key, redisValue).ConfigureAwait(false);
					await utility.SortedSetRemoveAsync(key, redisValue).ConfigureAwait(false);
					break;
				}
			}
		}

		if (user is null || !score.HasValue)
		{
			throw new ArgumentException(account);
		}

		user.Name = source.Name;

		await utility.SortedSetAddAsync(
			key,
			JsonSerializer.Serialize(user),
			score.Value,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}
}
