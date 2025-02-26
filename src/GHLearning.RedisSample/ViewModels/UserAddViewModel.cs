using System.ComponentModel.DataAnnotations;

namespace GHLearning.RedisSample.ViewModels;

public record UserAddViewModel
{
	[Required]
	public string Account { get; init; } = default!;
	[Required]
	public string Name { get; init; } = default!;
}
