namespace Twitter.Clone.Trends.Persistence;

public class TrendsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string HashtagsCollectionName { get; set; } = null!;
}
