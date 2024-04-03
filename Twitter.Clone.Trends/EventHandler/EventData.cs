namespace Twitter.Clone.Trends.EventHandler;

public record EventData
{
    public required string IPAddress { get; init; }
    public required List<string> Hashtags { get; init; }
}
