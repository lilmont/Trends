namespace Twitter.Clone.Trends.Handlers;

public class HashtagCommandHandler(
    HttpClient httpClient,
    IOptions<LocatorServiceSettings> locatorServiceOptions,
    HashtagRepository hashtagRepository)
    : IRequestHandler<HashtagCommand, bool>
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<LocatorServiceSettings> _locatorServiceOptions = locatorServiceOptions;
    private readonly HashtagRepository _hashtagRepository = hashtagRepository;
    public async Task<bool> Handle(HashtagCommand request, CancellationToken cancellationToken)
    {
        HashtagEventValidator validator = new();
        try
        {
            var hashtagContent = JsonSerializer.Deserialize<HashtagsEvent>(request.InboxContent);

            if (hashtagContent != null && hashtagContent.Hashtags.Any())
            {
                var validationResult = validator.Validate(hashtagContent);
                if (validationResult.IsValid)
                {
                    var response = await _httpClient
                        .GetAsync(_locatorServiceOptions.Value.URL + hashtagContent.IPAddress, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                    LocatorServiceResponse geoInformation = null!;

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        geoInformation = JsonSerializer.Deserialize<LocatorServiceResponse>(jsonResponse)!;
                    }

                    foreach (var hashtag in hashtagContent.Hashtags)
                    {
                        await _hashtagRepository.CreateAsync(
                            new Hashtag()
                            {
                                Name = hashtag,
                                DateCreated = DateTime.UtcNow,
                                IPAddress = hashtagContent.IPAddress,
                                Country = geoInformation?.CountryName,
                                Continent = geoInformation?.ContinentName
                            },
                            cancellationToken);
                    }
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
