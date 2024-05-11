namespace Twitter.Clone.Trends.Persistence;

public record TrendsDatabaseSettings
{
    public const string SectionName = "TrendsDatabase";
    public string Host { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}
