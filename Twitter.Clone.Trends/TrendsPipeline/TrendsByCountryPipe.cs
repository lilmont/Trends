namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsByCountryPipe : BasePipe
{
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings;
    private readonly TrendsByCountryRepository _trendsByCountryRepository;

    public TrendsByCountryPipe(Action<HashtagRepository, CancellationToken> next,
        IOptions<MakeTrendsSettings> makeTrendsSettings,
        TrendsByCountryRepository trendsByCountryRepository) : base(next)
    {
        _makeTrendsSettings = makeTrendsSettings;
        _trendsByCountryRepository = trendsByCountryRepository;
    }

    public async override void HandleAsync(HashtagRepository context, CancellationToken cancellationToken)
    {

        var trends = (await context.GetHashtagsByTimeSpanAsync(_makeTrendsSettings.Value.CountryTrendTimeSpan))
             .GroupBy(x => new { x.Name, x.Country })
             .Select(g => new TrendByCountry
             {
                 Name = g.Key.Name,
                 Country = g.Key.Country,
                 Count = g.Count()
             })
             .ToList();

        if (trends is not null)
        {
            foreach (var trend in trends)
            {
                if (await _trendsByCountryRepository.TrendExistsAsync(trend.Name, trend.Country, cancellationToken))
                    await _trendsByCountryRepository.UpdateAsync(trend.Name, trend.Country, trend.Count, cancellationToken);
                else
                    await _trendsByCountryRepository.CreateAsync(trend, cancellationToken);
            }
        }

        if (_next is not null) _next(context, cancellationToken);
    }
}
