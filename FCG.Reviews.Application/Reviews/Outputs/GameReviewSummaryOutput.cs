namespace FCG.Reviews.Application.Reviews.Outputs;

public record GameReviewSummaryOutput(
    Guid GameId,
    double AverageRating,
    int TotalReviews,
    IReadOnlyList<ReviewOutput> Reviews
);
