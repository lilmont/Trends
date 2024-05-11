namespace Twitter.Clone.Trends.Responses;

public class LocatorServiceResponse
{
    [JsonPropertyName("continentName")]
    public required string ContinentName { get; set; }

    [JsonPropertyName("countryName")]
    public required string CountryName { get; set; }
}
