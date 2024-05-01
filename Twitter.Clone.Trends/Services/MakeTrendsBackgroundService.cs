namespace Twitter.Clone.Trends.Services;

public class MakeTrendsBackgroundService(
    ILogger<MakeTrendsBackgroundService> logger,
    IOptions<MakeTrendsBackgroundServiceSettings> makeTrendsBackgroundServiceSettings,
    HashtagRepository hashtagRepository)
    : BackgroundService
{
    private readonly ILogger<MakeTrendsBackgroundService> _logger = logger;
    private readonly IOptions<MakeTrendsBackgroundServiceSettings> _makeTrendsBackgroundServiceSettings = makeTrendsBackgroundServiceSettings;
    private readonly HashtagRepository _hashtagRepository = hashtagRepository;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pipeline = new PipelineBuilder()
                    .AddPipe(typeof(TrendsByCountryPipe))
                    .AddPipe(typeof(TrendsByContinentPipe))
                    .Build();

                pipeline(_hashtagRepository);

                await Task.Delay(_makeTrendsBackgroundServiceSettings.Value.Frequency, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while carrying out background service: MakeTrendsBackgroundService");
        }
    }
}
