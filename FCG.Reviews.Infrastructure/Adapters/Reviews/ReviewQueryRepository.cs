using FCG.Reviews.Application.Reviews.Ports;
using FCG.Reviews.Domain.Reviews.Entities;
using FCG.Reviews.Infrastructure.Persistence;
using MongoDB.Driver;

namespace FCG.Reviews.Infrastructure.Adapters.Reviews;

public class ReviewQueryRepository : IReviewQueryRepository
{
    private readonly MongoDbContext _context;

    public ReviewQueryRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken)
    {
        return await _context.Reviews
            .Find(r => r.GameId == gameId)
            .Sort(Builders<Review>.Sort.Descending("createdAt"))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UserAlreadyReviewedAsync(Guid gameId, int userId, CancellationToken cancellationToken)
    {
        var count = await _context.Reviews
            .CountDocumentsAsync(r => r.GameId == gameId && r.UserId == userId, cancellationToken: cancellationToken);

        return count > 0;
    }
}
