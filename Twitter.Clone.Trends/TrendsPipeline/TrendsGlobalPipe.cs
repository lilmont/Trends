namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsGlobalPipe : BasePipe
{
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings;
    private readonly TrendsGlobalRepository _trendsGlobalRepository;

    public TrendsGlobalPipe(Action<HashtagRepository, CancellationToken> next,
    IOptions<MakeTrendsSettings> makeTrendsSettings,
    TrendsGlobalRepository trendsGlobalRepository) : base(next)
    {
        _makeTrendsSettings = makeTrendsSettings;
        _trendsGlobalRepository = trendsGlobalRepository;
    }

    public async override void HandleAsync(HashtagRepository context, CancellationToken cancellationToken)
    {
        var trends = (await context.GetHashtagsByTimeSpanAsync(_makeTrendsSettings.Value.GlobalTrendTimeSpan))
             .GroupBy(x => new { x.Name })
             .Take(10)
             .Select(g => new TrendGlobal
             {
                 Name = g.Key.Name,
                 Count = g.Count()
             })
             .ToList();

        if (trends is not null)
        {
            foreach (var trend in trends)
            {
                if (await _trendsGlobalRepository.TrendExistsAsync(trend.Name, cancellationToken))
                    await _trendsGlobalRepository.UpdateAsync(trend.Name, trend.Count, cancellationToken);
                else
                    await _trendsGlobalRepository.CreateAsync(trend, cancellationToken);
            }
        }

        if (_next is not null) _next(context, cancellationToken);
    }
}
