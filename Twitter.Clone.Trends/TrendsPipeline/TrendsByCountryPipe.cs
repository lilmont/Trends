namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsByCountryPipe(
    Action<HashtagRepository> next,
    IOptions<MakeTrendsSettings> makeTrendsSettings,
    TrendsByCountryRepository trendsByCountryRepository)
    : BasePipe(next)
{
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings = makeTrendsSettings;
    private readonly TrendsByCountryRepository _trendsByCountryRepository = trendsByCountryRepository;

    public async override void Handle(HashtagRepository context)
    {
        var trends = (await context.GetHashtagsByTimeSpanAsync(_makeTrendsSettings.Value.CountryTrendTimeSpan))
             .GroupBy(x => new { x.Name, x.Country })
             .Select(g => new TrendsByCountry
             {
                 Name = g.Key.Name,
                 Country = g.Key.Country,
                 Count = g.Count()
             })
             .ToList();

        foreach (var trend in trends)
        {
            if (await trendsByCountryRepository.TrendExistsAsync(trend.Name, trend.Country))
                await _trendsByCountryRepository.UpdateAsync(trend.Name, trend.Country, trend.Count);
            else
                await _trendsByCountryRepository.CreateAsync(trend);
        }
    }
}
