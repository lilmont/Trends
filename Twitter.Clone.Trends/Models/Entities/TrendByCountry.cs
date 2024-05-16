namespace Twitter.Clone.Trends.Models.Entities;

public class TrendByCountry
{
    public const string CollectionName = "TrendsByCountry";

    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required int Count { get; set; }
}
