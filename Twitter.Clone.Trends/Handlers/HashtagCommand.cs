namespace Twitter.Clone.Trends.Handlers;

public class HashtagCommand : IRequest<bool>
{
    public required string InboxContent { get; set; }
}
