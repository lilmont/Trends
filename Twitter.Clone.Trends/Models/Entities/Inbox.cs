namespace Twitter.Clone.Trends.Models.Entities;

public class Inbox
{
    public const string CollectionName = "Inboxes";

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public required string MessageType { get; set; }
    public required string Content { get; set; }
    public required DateTime DateCreated { get; set; }
    public bool IsProcessed { get; set; } = false;

    public static Inbox CreateMessage<TModel>(TModel model)
    {
        return new Inbox()
        {
            MessageType = typeof(TModel).FullName!,
            Content = System.Text.Json.JsonSerializer.Serialize(model),
            DateCreated = DateTime.UtcNow
        };
    }
}

