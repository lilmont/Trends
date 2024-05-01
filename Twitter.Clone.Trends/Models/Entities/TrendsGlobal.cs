namespace Twitter.Clone.Trends.Models.Entities;

public class TrendsGlobal
{
    public const string CollectionName = "TrendsGlobal";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required int Count { get; set; }
}
