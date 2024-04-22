namespace Twitter.Clone.Trends.AppSettings;

public sealed class AddGeoDataBackgroundServiceSettings
{
    public const string SectionName = "AddGeoDataBackgroundService";

    public required int Frequency { get; set; }
}
