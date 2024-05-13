namespace Twitter.Clone.Trends.Models.Entities;

public class TrendByContinent
{
    public const string CollectionName = "TrendsByContinent";

    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required string Continent { get; set; }
    public required int Count { get; set; }
}
