namespace Twitter.Clone.Trends.EventHandler;

public record EventBrokerSettings
{
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
