namespace Twitter.Clone.Trends.EventHandler;

public record HashtagsEvent(string IPAddress, List<string> Hashtags);

