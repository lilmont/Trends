

namespace Twitter.Clone.Trends.Services;

public class InboxBackgroundService(
    InboxHashtagRepository inboxHashtagRepository,
    IOptions<InboxBackgroundServiceSettings> inboxBackgroundServiceSettings,
    ILogger<InboxBackgroundService> logger,
    InboxProcessor inboxProcessor)
    : BackgroundService
{
    private readonly InboxHashtagRepository _inboxHashtagRepository = inboxHashtagRepository;
    private readonly IOptions<InboxBackgroundServiceSettings> _inboxBackgroundServiceSettings = inboxBackgroundServiceSettings;
    private readonly ILogger<InboxBackgroundService> _logger = logger;
    private readonly InboxProcessor _inboxProcessor = inboxProcessor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var unprocessedInbox = await _inboxHashtagRepository.GetUnprocessedInboxAsync(stoppingToken);

                foreach (var record in unprocessedInbox)
                {
                    var success = await _inboxProcessor.ProcessMessageAsync(record.MessageType, record.Content, stoppingToken);

                    if (success)
                    {
                        await _inboxHashtagRepository.UpdateProcessedStatusAsync(record.Id, stoppingToken);
                    }
                }
                await Task.Delay(_inboxBackgroundServiceSettings.Value.Frequency, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while carrying out background service: TrendsMakerBackgroundService");
        }
    }
}
