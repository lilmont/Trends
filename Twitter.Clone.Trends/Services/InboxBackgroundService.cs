namespace Twitter.Clone.Trends.Services;

public class InboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<InboxBackgroundServiceSettings> inboxBackgroundServiceSettings,
    ILogger<InboxBackgroundService> logger)
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IOptions<InboxBackgroundServiceSettings> _inboxBackgroundServiceSettings = inboxBackgroundServiceSettings;
    private readonly ILogger<InboxBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var inboxHashtagRepository = scope.ServiceProvider.GetRequiredService<InboxHashtagRepository>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var unprocessedInbox = await inboxHashtagRepository.GetUnprocessedInboxAsync(stoppingToken);

                    foreach (var message in unprocessedInbox)
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        var type = assembly.GetType(message.MessageType);

                        if (type is not null)
                        {
                            var msg = JsonConvert.DeserializeObject(message.Content, type);
                            if (msg is INotification notify)
                                await mediator.Publish(notify, stoppingToken);
                        }

                        await inboxHashtagRepository.UpdateProcessedStatusAsync(message.Id, stoppingToken);
                    }
                }
                await Task.Delay(_inboxBackgroundServiceSettings.Value.Frequency, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while carrying out background service: InboxBackgroundService");
        }
    }
}
