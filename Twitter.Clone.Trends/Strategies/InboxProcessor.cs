namespace Twitter.Clone.Trends.Strategies;

public class InboxProcessor
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, Func<string, IRequest<bool>>> _strategies;

    public InboxProcessor(IMediator mediator)
    {
        _mediator = mediator;
        _strategies = new Dictionary<string, Func<string, IRequest<bool>>>
        {
            { nameof(HashtagsEvent), content => new HashtagCommand() { InboxContent = content} },
        };
    }

    public async Task<bool> ProcessMessageAsync(string messageType, string content, CancellationToken cancellationToken)
    {
        if (_strategies.TryGetValue(messageType, out var strategy))
        {
            var request = strategy(content);
            return await _mediator.Send(request, cancellationToken);
        }

        return false;
    }
}
