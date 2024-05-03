namespace Twitter.Clone.Trends.TrendsPipeline;

public class PipelineBuilder
{
    private Dictionary<Type, object> _pipes = new Dictionary<Type, object>();
    public PipelineBuilder AddPipe(Type type, object repository)
    {
        _pipes.Add(type, repository);
        return this;
    }

    public Action<HashtagRepository, CancellationToken> Build(IOptions<MakeTrendsSettings> makeTrendsSettings)
    {
        var lastIndex = _pipes.Count - 1;
        var selectedPipe = (BasePipe)Activator.CreateInstance(_pipes.ElementAt(lastIndex).Key, new object[] { null, makeTrendsSettings, _pipes.ElementAt(lastIndex).Value });
        for (int i = lastIndex - 1; i > 0; i--)
        {
            selectedPipe = (BasePipe)Activator.CreateInstance(_pipes.ElementAt(i).Key, new object[] { selectedPipe.HandleAsync, makeTrendsSettings, _pipes.ElementAt(i).Value });
        }
        var firstPipe = (BasePipe)Activator.CreateInstance(_pipes.ElementAt(0).Key, new[] { selectedPipe.HandleAsync, makeTrendsSettings, _pipes.ElementAt(0).Value });
        return firstPipe.HandleAsync;
    }
}
