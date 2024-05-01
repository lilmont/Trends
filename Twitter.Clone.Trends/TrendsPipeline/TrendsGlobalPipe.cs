namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsGlobalPipe(Action<HashtagRepository> next) : BasePipe(next)
{
    public override void Handle(HashtagRepository context)
    {

    }
}
