namespace Twitter.Clone.Trends.Models.Entities;

public class TrendGlobal
{
    public const string CollectionName = "TrendsGlobal";

    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required int Count { get; set; }
}
