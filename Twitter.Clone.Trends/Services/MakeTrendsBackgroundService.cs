namespace Twitter.Clone.Trends.Services;

public class MakeTrendsBackgroundService(
    ILogger<MakeTrendsBackgroundService> logger,
    IOptions<MakeTrendsBackgroundServiceSettings> makeTrendsBackgroundServiceSettings,
    HashtagRepository hashtagRepository,
    IOptions<MakeTrendsSettings> makeTrendsSettings,
    TrendsByContinentRepository trendsByContinentRepository,
    TrendsByCountryRepository trendsByCountryRepository,
    TrendsGlobalRepository trendsGlobalRepository)
    : BackgroundService
{
    private readonly ILogger<MakeTrendsBackgroundService> _logger = logger;
    private readonly IOptions<MakeTrendsBackgroundServiceSettings> _makeTrendsBackgroundServiceSettings = makeTrendsBackgroundServiceSettings;
    private readonly HashtagRepository _hashtagRepository = hashtagRepository;
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings = makeTrendsSettings;
    private readonly TrendsByContinentRepository _trendsByContinentRepository = trendsByContinentRepository;
    private readonly TrendsByCountryRepository _trendsByCountryRepository = trendsByCountryRepository;
    private readonly TrendsGlobalRepository _trendsGlobalRepository = trendsGlobalRepository;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pipeline = new PipelineBuilder()
                    .AddPipe(typeof(TrendsByCountryPipe), _trendsByCountryRepository)
                    .AddPipe(typeof(TrendsByContinentPipe), _trendsByContinentRepository)
                    .AddPipe(typeof(TrendsGlobalPipe), _trendsGlobalRepository)
                    .Build(_makeTrendsSettings);

                pipeline(_hashtagRepository,stoppingToken);

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
