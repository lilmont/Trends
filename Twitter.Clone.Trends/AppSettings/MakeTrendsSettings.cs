namespace Twitter.Clone.Trends.AppSettings;

public class MakeTrendsSettings
{
    public const string SectionName = "MakeTrends";

    public required int CountryTrendTimeSpan { get; set; }
    public required int ContinentTrendTimeSpan { get; set; }
    public required int GlobalTrendTimeSpan { get; set; }
}
