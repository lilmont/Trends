namespace Twitter.Clone.Trends.EventHandler;

public class EventConsumer(HashtagsService hashtagsService) : IConsumer<EventData>
{
    private readonly HashtagsService _hashtagsService = hashtagsService;

    public async Task Consume(ConsumeContext<EventData> context)
    {
        if (!string.IsNullOrEmpty(context.Message.IPAddress) &&
            context.Message.Hashtags.Count > 0)
            foreach (var hashtag in context.Message.Hashtags)
                await _hashtagsService.CreateAsync(new Hashtag
                {
                    IPAddress = context.Message.IPAddress,
                    Name = hashtag,
                    DateCreated = DateTime.UtcNow,
                    IsProcessed = false,
                });
    }
}
