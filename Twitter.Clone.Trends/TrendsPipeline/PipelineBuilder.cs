namespace Twitter.Clone.Trends.TrendsPipeline;

public class PipelineBuilder
{
    private List<Type> _pipes = new List<Type>();
    public PipelineBuilder AddPipe(Type type)
    {
        _pipes.Add(type);
        return this;
    }

    public Action<HashtagRepository,CancellationToken> Build()
    {
        var lastIndex = _pipes.Count - 1;
        var selectedPipe = (BasePipe)Activator.CreateInstance(_pipes[lastIndex], null);
        for (int i = lastIndex - 1; i > 0; i--)
        {
            selectedPipe = (BasePipe)Activator.CreateInstance(_pipes[i], new[] { selectedPipe.HandleAsync });
        }
        var firstPipe = (BasePipe)Activator.CreateInstance(_pipes[0], new[] { selectedPipe.HandleAsync });
        return firstPipe.HandleAsync;
    }
}
