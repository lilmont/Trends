namespace Twitter.Clone.Trends.EventHandler;

public record HashtagsEvent(string IPAddress, List<string> Hashtags) : INotification
{
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}


