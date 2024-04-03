namespace Twitter.Clone.Trends.EventHandler;

public record EventData
{
    public required string IPAddress { get; set; }
    public required List<string> Hashtags { get; set; }
}
