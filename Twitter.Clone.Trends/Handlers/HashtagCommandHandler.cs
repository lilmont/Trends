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
                    .GetAsync(_locatorServiceOptions.Value.URL + request.IPAddress, cancellationToken);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                LocatorServiceResponse geoInformation = null!;

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    geoInformation = System.Text.Json.JsonSerializer.Deserialize<LocatorServiceResponse>(jsonResponse)!;
                }

                foreach (var hashtag in request.Hashtags)
                {
                    await _hashtagRepository.CreateAsync(
                        new Hashtag()
                        {
                            Name = hashtag,
                            DateCreated = DateTime.UtcNow,
                            IPAddress = request.IPAddress,
                            Country = geoInformation?.CountryName,
                            Continent = geoInformation?.ContinentName
                        },
                        cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
               "Error occurred in HashtagCommandHandler for entry: {0}", request);
        }
    }
}
