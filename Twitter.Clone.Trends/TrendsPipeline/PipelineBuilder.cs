namespace Twitter.Clone.Trends.TrendsPipeline;

public class PipelineBuilder(ILogger<PipelineBuilder> logger)
{
    private readonly ILogger<PipelineBuilder> _logger = logger;

    private readonly Dictionary<Type, object> _pipes = [];
    public PipelineBuilder AddPipe(Type type, object repository)
    {
        _pipes.Add(type, repository);
        return this;
    }

    public Action<HashtagRepository, CancellationToken> Build(IOptions<MakeTrendsSettings> makeTrendsSettings)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex,
               "Error occurred when building the pipeline" + ex.Message);
            return (repo, token) => {  };
        }
    }
}
