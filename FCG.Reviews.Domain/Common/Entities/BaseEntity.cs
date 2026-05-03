using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Reviews.Domain.Common.Entities;

public abstract class BaseEntity
{
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}
