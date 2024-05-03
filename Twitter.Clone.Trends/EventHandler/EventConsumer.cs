namespace Twitter.Clone.Trends.EventHandler;

public class EventConsumer(HashtagRepository hashtagsService,
    ILogger<EventConsumer> logger)
    : IConsumer<HashtagsEvent>
{
    private readonly HashtagRepository _hashtagsService = hashtagsService;
    private readonly ILogger<EventConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<HashtagsEvent> context)
    {
        //try
        //{
        //    if (!string.IsNullOrEmpty(context.Message.IPAddress) &&
        //        context.Message.Hashtags is not null &&
        //        context.Message.Hashtags.Count > 0)
        //    {
        //        foreach (var hashtag in context.Message.Hashtags)
        //        {
        //            await _hashtagsService.CreateHashtagAsync(
        //                new Hashtag
        //                {
        //                    //IPAddress = context.Message.IPAddress,
        //                    Name = hashtag,
        //                    //DateCreated = DateTime.UtcNow,
        //                    //IsProcessed = false,
        //                },
        //                context.CancellationToken);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex,
        //        "Error occurred while processing the message with IP: {IPAddress}",
        //        context.Message.IPAddress);
        //}
    }
}
