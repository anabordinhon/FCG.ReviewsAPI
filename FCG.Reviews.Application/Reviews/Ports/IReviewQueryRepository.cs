using FCG.Reviews.Domain.Reviews.Entities;

namespace FCG.Reviews.Application.Reviews.Ports;

public interface IReviewQueryRepository
{
    Task<List<Review>> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken);
    Task<bool> UserAlreadyReviewedAsync(Guid gameId, int userId, CancellationToken cancellationToken);
}
