namespace Twitter.Clone.Trends.Models.Entities;

public record Hashtag
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string IPAddress { get; set; }
    public bool IsProcessed { get; set; }
}
