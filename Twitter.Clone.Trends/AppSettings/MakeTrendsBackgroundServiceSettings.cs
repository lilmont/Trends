namespace Twitter.Clone.Trends.AppSettings;

public sealed class MakeTrendsBackgroundServiceSettings
{
    public const string SectionName = "MakeTrendsBackgroundService";

    public required int Frequency { get; set; }
}
