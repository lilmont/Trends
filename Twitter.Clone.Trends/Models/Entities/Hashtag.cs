using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Twitter.Clone.Trends.Models.Entities;

public record Hashtag
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string IPAddress { get; set; }
    public bool IsProcessed { get; set; }
}
