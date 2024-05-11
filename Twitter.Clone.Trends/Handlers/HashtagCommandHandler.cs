namespace Twitter.Clone.Trends.Handlers;

public class HashtagCommandHandler(
    HttpClient httpClient,
    IOptions<LocatorServiceSettings> locatorServiceOptions,
    HashtagRepository hashtagRepository,
    ILogger<HashtagCommandHandler> logger)
    : INotificationHandler<HashtagsEvent>
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<LocatorServiceSettings> _locatorServiceOptions = locatorServiceOptions;
    private readonly HashtagRepository _hashtagRepository = hashtagRepository;
    private readonly ILogger<HashtagCommandHandler> _logger = logger;
    public async Task Handle(HashtagsEvent request, CancellationToken cancellationToken)
    {
        HashtagEventValidator validator = new();
        try
        {
            var validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                var response = await _httpClient
                    .GetFromJsonAsync<LocatorServiceResponse>(_locatorServiceOptions.Value.URL + request.IPAddress, cancellationToken);

                if (response is null) throw new NullReferenceException("Locator Service response is null!");

                foreach (var hashtag in request.Hashtags)
                {
                    await _hashtagRepository.CreateAsync(
                        new Hashtag
                        {
                            Name = hashtag,
                            DateCreated = DateTime.UtcNow,
                            IPAddress = request.IPAddress,
                            Country = response.CountryName,
                            Continent = response.ContinentName
                        },
                        cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
               "Error occurred in HashtagCommandHandler for entry: {request}", request);
        }
    }
}
