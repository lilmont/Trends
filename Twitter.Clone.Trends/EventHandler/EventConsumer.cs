using MassTransit;

namespace Twitter.Clone.Trends.EventHandler;

public class EventConsumer : IConsumer<EventData>
{
    public async Task Consume(ConsumeContext<EventData> context)
    {
        // logic
    }
}
