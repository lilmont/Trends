namespace Twitter.Clone.Trends.Models.Entities;

public class Hashtag
{
    public const string CollectionName = "Hashtags";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonRequired]
    public string Name { get; set; }

    public List<HashtagLog>? Log { get; set; }

}

public class HashtagLog
{
    public const string CollectionName = "HashtagLogs";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonRequired]
    public string HashtagId { get; set; }

    [BsonRequired]
    public DateTime DateCreated { get; set; }

    [BsonRequired]
    public string IPAddress { get; set; }

    public bool IsProcessed { get; set; }

    public int Count { get; set; }
}

