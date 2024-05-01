namespace Twitter.Clone.Trends.TrendsPipeline;

public abstract class BasePipe
{
    protected readonly Action<HashtagRepository> _next;
    public BasePipe(Action<HashtagRepository> next)
    {
        _next = next;
    }
    public abstract void Handle(HashtagRepository context);
}
