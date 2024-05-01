namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsByContinentPipe(Action<HashtagRepository> next) : BasePipe(next)
{
    public override void Handle(HashtagRepository context)
    {

    }
}
