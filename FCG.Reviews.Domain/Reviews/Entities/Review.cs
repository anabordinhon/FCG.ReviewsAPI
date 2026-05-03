using FCG.Reviews.Domain.Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Reviews.Domain.Reviews.Entities;

public class Review : BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }

    public Guid GameId { get; private set; }
    public int UserId { get; private set; }

    [BsonRepresentation(BsonType.Int32)]
    public int Rating { get; private set; }

    public string Comment { get; private set; } = string.Empty;
    private Review() { }

    public static Review Create(Guid gameId, int userId, int rating, string comment, DateTime createdAt)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("A nota deve ser entre 1 e 5.", nameof(rating));

        return new Review
        {
            GameId = gameId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = createdAt
        };
    }
}
