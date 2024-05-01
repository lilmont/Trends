namespace Twitter.Clone.Trends.Services;

public class MakeTrendsBackgroundService(
    ILogger<MakeTrendsBackgroundService> logger,
    IOptions<MakeTrendsBackgroundServiceSettings> makeTrendsBackgroundServiceSettings)
    : BackgroundService
{
    private readonly ILogger<MakeTrendsBackgroundService> _logger = logger;
    private readonly IOptions<MakeTrendsBackgroundServiceSettings> _makeTrendsBackgroundServiceSettings = makeTrendsBackgroundServiceSettings;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pipeline = new PipelineBuilder()
                    .AddPipe(typeof(TrendsByCountryPipe))
                    .Build();

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
