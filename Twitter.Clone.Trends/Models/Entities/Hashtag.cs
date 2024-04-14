namespace Twitter.Clone.Trends.Models.Entities;

public class Hashtag
{
    public const string CollectionName = "Hashtags";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }

    public List<HashtagLog>? Log { get; set; }
}

public class HashtagLog
{
    public const string CollectionName = "HashtagLogs";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    public required string HashtagId { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string IPAddress { get; set; }
    public bool IsProcessed { get; set; }
}

