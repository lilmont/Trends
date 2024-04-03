namespace Twitter.Clone.Trends.EventHandler;

public class EventConsumer(HashtagsService hashtagsService,
    ILogger<EventConsumer> logger)
    : IConsumer<EventData>
{
    private readonly HashtagsService _hashtagsService = hashtagsService;
    private readonly ILogger<EventConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<EventData> context)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                $"Error occurred while processing the message with IP: {context.Message.IPAddress}");
        }
    }
}
