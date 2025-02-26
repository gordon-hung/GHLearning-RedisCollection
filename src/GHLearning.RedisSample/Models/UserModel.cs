namespace GHLearning.RedisSample.Models;

public record UserModel
{
	public string Account { get; set; } = default!;
	public string Name { get; set; } = default!;
}
