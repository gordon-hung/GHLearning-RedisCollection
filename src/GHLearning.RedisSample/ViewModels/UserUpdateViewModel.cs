using System.ComponentModel.DataAnnotations;

namespace GHLearning.RedisSample.ViewModels;

public record UserUpdateViewModel
{
	[Required]
	public string Name { get; init; } = default!;
}
