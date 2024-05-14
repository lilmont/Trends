namespace Twitter.Clone.Trends.TrendsPipeline;

public abstract class BasePipe(Action<HashtagRepository, CancellationToken> next)
{
    protected readonly Action<HashtagRepository, CancellationToken> _next = next;

    public abstract void HandleAsync(HashtagRepository context, CancellationToken cancellationToken);
}
