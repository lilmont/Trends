using MassTransit;
using Twitter.Clone.Trends.Models.Entities;
using Twitter.Clone.Trends.Services;

namespace Twitter.Clone.Trends.EventHandler;

public class EventConsumer : IConsumer<EventData>
{
    private readonly HashtagsService _hashtagsService;

    public EventConsumer(HashtagsService hashtagsService)
    {
        _hashtagsService = hashtagsService;
    }
    public async Task Consume(ConsumeContext<EventData> context)
    {
        if (!string.IsNullOrEmpty(context.Message.IPAddress) &&
            context.Message.Hashtags.Count() > 0)
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
