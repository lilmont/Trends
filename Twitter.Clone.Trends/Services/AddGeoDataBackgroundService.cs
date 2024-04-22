namespace Twitter.Clone.Trends.Services;

public class AddGeoDataBackgroundService(
    HashtagRepository hashtagRepository,
    HttpClient httpClient,
    IOptions<LocatorServiceSettings> LocatorServiceOptions,
    IOptions<AddGeoDataBackgroundServiceSettings> AddGeoDataBackgroundServiceOptions,
    ILogger<AddGeoDataBackgroundService> logger)
    : BackgroundService
{
    private readonly HashtagRepository _hashtagRepository = hashtagRepository;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<LocatorServiceSettings> _LocatorServiceOptions = LocatorServiceOptions;
    private readonly IOptions<AddGeoDataBackgroundServiceSettings> _AddGeoDataBackgroundServiceOptions = AddGeoDataBackgroundServiceOptions;
    private readonly ILogger<AddGeoDataBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var hashtagsWithoutGeoInfo = await _hashtagRepository.GetHashtagsWithoutGeoInfoAsync(stoppingToken);

                foreach (var hashtag in hashtagsWithoutGeoInfo)
                {
                    var response = await _httpClient
                        .GetAsync(_LocatorServiceOptions.Value.URL + hashtag.IPAddress, stoppingToken);

                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync(stoppingToken);

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        var GeoInformation = JsonSerializer.Deserialize<LocatorServiceResponse>(jsonResponse)!;
                        await _hashtagRepository.UpdateGeoInfoAsync(hashtag.Id, GeoInformation.CountryName, GeoInformation.ContinentName, stoppingToken);
                    }
                }
                await Task.Delay(_AddGeoDataBackgroundServiceOptions.Value.Frequency, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while carrying out background service: TrendsMakerBackgroundService");
        }
    }
}
