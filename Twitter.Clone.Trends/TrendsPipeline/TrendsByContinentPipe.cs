namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsByContinentPipe : BasePipe
{
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings;
    private readonly TrendsByContinentRepository _trendsByContinentRepository;

    public TrendsByContinentPipe(Action<HashtagRepository, CancellationToken> next,
        IOptions<MakeTrendsSettings> makeTrendsSettings,
        TrendsByContinentRepository trendsByContinentRepository) : base(next)
    {
        _makeTrendsSettings = makeTrendsSettings;
        _trendsByContinentRepository = trendsByContinentRepository;
    }

    public async override void HandleAsync(HashtagRepository context, CancellationToken cancellationToken)
    {
        var trends = (await context.GetHashtagsByTimeSpanAsync(_makeTrendsSettings.Value.ContinentTrendTimeSpan))
             .GroupBy(x => new { x.Name, x.Continent })
             .Select(g => new TrendByContinent
             {
                 Name = g.Key.Name,
                 Continent = g.Key.Continent,
                 Count = g.Count()
             })
             .ToList();

        if (trends is not null)
        {
            foreach (var trend in trends)
            {
                if (await _trendsByContinentRepository.TrendExistsAsync(trend.Name, trend.Continent, cancellationToken))
                    await _trendsByContinentRepository.UpdateAsync(trend.Name, trend.Continent, trend.Count, cancellationToken);
                else
                    await _trendsByContinentRepository.CreateAsync(trend, cancellationToken);
            }
        }

        if (_next is not null) _next(context, cancellationToken);
    }
}
