namespace FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;

public record AddReviewCommand(Guid GameId, int UserId, int Rating, string Comment, DateTime CreatedAt);
