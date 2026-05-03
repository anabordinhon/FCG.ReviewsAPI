using FCG.Reviews.Domain.Reviews.Entities;

namespace FCG.Reviews.Domain.Reviews.Ports;

public interface IReviewQueryRepository
{
    Task<List<Review>> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken);
    Task<bool> UserAlreadyReviewedAsync(Guid gameId, Guid userId, CancellationToken cancellationToken);
}
