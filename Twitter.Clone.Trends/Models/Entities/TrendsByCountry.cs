namespace Twitter.Clone.Trends.Models.Entities;

public class TrendsByCountry
{
    public const string CollectionName = "TrendsByCountry";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required int Count { get; set; }
}
