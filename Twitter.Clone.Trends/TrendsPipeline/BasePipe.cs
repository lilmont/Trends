namespace Twitter.Clone.Trends.TrendsPipeline;

public abstract class BasePipe
{
    protected readonly Action<HashtagRepository, CancellationToken> _next;
    public BasePipe(Action<HashtagRepository, CancellationToken> next)
    {
        _next = next;
    }
    public abstract void HandleAsync(HashtagRepository context, CancellationToken cancellationToken);
}
