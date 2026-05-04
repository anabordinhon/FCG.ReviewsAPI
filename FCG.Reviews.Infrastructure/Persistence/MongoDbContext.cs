using FCG.Reviews.Domain.Reviews.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace FCG.Reviews.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    // MongoDbContext.cs
    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);

        var indexKeys = Builders<Review>.IndexKeys
            .Ascending(r => r.GameId)
            .Ascending(r => r.UserId);
        Reviews.Indexes.CreateOne(new CreateIndexModel<Review>(indexKeys));
    }

    public IMongoCollection<Review> Reviews =>
        _database.GetCollection<Review>("game_reviews");
}
