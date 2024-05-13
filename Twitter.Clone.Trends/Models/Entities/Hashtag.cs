namespace Twitter.Clone.Trends.Models.Entities;

public class Hashtag
{
    public const string CollectionName = "Hashtags";

    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public required DateTime DateCreated { get; set; }
    public required string IPAddress { get; set; }
    public string? Country { get; set; }
    public string? Continent { get; set; }
}

