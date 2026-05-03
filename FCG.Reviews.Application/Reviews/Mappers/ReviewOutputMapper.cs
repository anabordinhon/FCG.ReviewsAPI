using FCG.Reviews.Application.Reviews.Outputs;
using FCG.Reviews.Domain.Reviews.Entities;

namespace FCG.Reviews.Application.Reviews.Mappers;

public static class ReviewOutputMapper
{
    public static ReviewOutput ToOutput(this Review review) =>
        new(review.Id!, review.GameId, review.UserId, review.Rating, review.Comment, review.CreatedAt);
}
