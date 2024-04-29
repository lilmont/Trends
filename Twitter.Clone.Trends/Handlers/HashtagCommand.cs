namespace Twitter.Clone.Trends.Handlers;

public class HashtagCommand : IRequest<bool>
{
    public required Inbox Inbox { get; set; }
}
