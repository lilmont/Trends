namespace Twitter.Clone.Trends.Services;

public class InboxBackgroundService(
    InboxHashtagRepository inboxHashtagRepository,
    IOptions<InboxBackgroundServiceSettings> inboxBackgroundServiceSettings,
    ILogger<InboxBackgroundService> logger,
    IMediator mediator)
    : BackgroundService
{
    private readonly InboxHashtagRepository _inboxHashtagRepository = inboxHashtagRepository;
    private readonly IOptions<InboxBackgroundServiceSettings> _inboxBackgroundServiceSettings = inboxBackgroundServiceSettings;
    private readonly ILogger<InboxBackgroundService> _logger = logger;
    private readonly IMediator _mediator = mediator;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var unprocessedInbox = await _inboxHashtagRepository.GetUnprocessedInboxAsync(stoppingToken);

                foreach (var message in unprocessedInbox)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var type = assembly.GetType(message.MessageType);

                    if (type is not null)
                    {
                        var msg = JsonConvert.DeserializeObject(message.Content, type);
                        if (msg is INotification notify)
                            await _mediator.Publish(notify, stoppingToken);
                    }

                    await _inboxHashtagRepository.UpdateProcessedStatusAsync(message.Id, stoppingToken);
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
