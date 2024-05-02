namespace Twitter.Clone.Trends.AppSettings;

public sealed class InboxBackgroundServiceSettings
{
    public const string SectionName = "InboxBackgroundService";

    public required int Frequency { get; set; }
}
