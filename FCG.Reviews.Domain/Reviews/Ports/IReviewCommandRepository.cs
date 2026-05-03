using FCG.Reviews.Domain.Reviews.Entities;

namespace FCG.Reviews.Domain.Reviews.Ports;

public interface IReviewCommandRepository
{
    Task<Review> AddAsync(Review review, CancellationToken cancellationToken);
}
