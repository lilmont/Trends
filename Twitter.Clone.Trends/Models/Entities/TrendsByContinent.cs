namespace Twitter.Clone.Trends.Models.Entities;

public class TrendsByContinent
{
    public const string CollectionName = "TrendsByContinent";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required string Continent { get; set; }
    public required int Count { get; set; }
}
