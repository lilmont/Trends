using FluentValidation;
using Twitter.Clone.Trends.Models.Validators;

namespace Twitter.Clone.Trends.EventHandler;

public class HashtagConsumer(HashtagRepository hashtagsService,
    ILogger<HashtagConsumer> logger)
    : IConsumer<HashtagsEvent>
{
    private readonly HashtagRepository _hashtagsService = hashtagsService;
    private readonly ILogger<HashtagConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<HashtagsEvent> context)
    {
        HashtagEventValidator validator = new();
        var validationResult = validator.Validate(context.Message);
        try
        {
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    _logger.LogError("Validation error on property {PropertyName}: {ErrorMessage}",
                        failure.PropertyName, failure.ErrorMessage);
                }
                return;
            }

            foreach (var hashtag in context.Message.Hashtags)
                await _hashtagsService.CreateAsync(
                    new Hashtag
                    {
                        IPAddress = context.Message.IPAddress,
                        Name = hashtag,
                        DateCreated = DateTime.UtcNow,
                    },
                    context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while processing the message with IP: {IPAddress}",
                context.Message.IPAddress);
        }
    }
}
