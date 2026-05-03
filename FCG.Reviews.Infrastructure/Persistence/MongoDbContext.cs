using FCG.Reviews.Domain.Reviews.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace FCG.Reviews.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
    }

    public IMongoCollection<Review> Reviews =>
        _database.GetCollection<Review>("game_reviews");
}
