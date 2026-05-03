namespace FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;

public class AddReviewInput
{
    public required Guid GameId { get; set; }
    public required int Rating { get; set; }
    public required string Comment { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AddReviewCommand MapToCommand(int userId) =>
        new(GameId, userId, Rating, Comment, CreatedAt);
}
