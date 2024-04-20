namespace Twitter.Clone.Trends.Models.Entities;

public class Hashtag
{
    public const string CollectionName = "Hashtags";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string IPAddress { get; set; }
}

