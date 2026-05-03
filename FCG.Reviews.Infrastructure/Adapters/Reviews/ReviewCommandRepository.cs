using FCG.Reviews.Application.Reviews.Ports;
using FCG.Reviews.Domain.Reviews.Entities;
using FCG.Reviews.Infrastructure.Persistence;

namespace FCG.Reviews.Infrastructure.Adapters.Reviews;

public class ReviewCommandRepository : IReviewCommandRepository
{
    private readonly MongoDbContext _context;

    public ReviewCommandRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Review> AddAsync(Review review, CancellationToken cancellationToken)
    {
        await _context.Reviews.InsertOneAsync(review, cancellationToken: cancellationToken);
        return review;
    }
}
