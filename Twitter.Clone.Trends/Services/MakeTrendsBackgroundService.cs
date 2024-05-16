namespace Twitter.Clone.Trends.Services;

public class MakeTrendsBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<MakeTrendsBackgroundService> logger,
    IOptions<MakeTrendsBackgroundServiceSettings> makeTrendsBackgroundServiceSettings,
    IOptions<MakeTrendsSettings> makeTrendsSettings)
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<MakeTrendsBackgroundService> _logger = logger;
    private readonly IOptions<MakeTrendsBackgroundServiceSettings> _makeTrendsBackgroundServiceSettings = makeTrendsBackgroundServiceSettings;
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings = makeTrendsSettings;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var hashtagRepository = scope.ServiceProvider.GetRequiredService<HashtagRepository>();
                    var trendsByContinentRepository = scope.ServiceProvider.GetRequiredService<TrendsByContinentRepository>();
                    var trendsByCountryRepository = scope.ServiceProvider.GetRequiredService<TrendsByCountryRepository>();
                    var trendsGlobalRepository = scope.ServiceProvider.GetRequiredService<TrendsGlobalRepository>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<PipelineBuilder>>();

                    var pipeline = new PipelineBuilder(logger)
                        .AddPipe(typeof(TrendsByCountryPipe), trendsByCountryRepository)
                        .AddPipe(typeof(TrendsByContinentPipe), trendsByContinentRepository)
                        .AddPipe(typeof(TrendsGlobalPipe), trendsGlobalRepository)
                        .Build(_makeTrendsSettings);

                    pipeline(hashtagRepository, stoppingToken);
                }

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
