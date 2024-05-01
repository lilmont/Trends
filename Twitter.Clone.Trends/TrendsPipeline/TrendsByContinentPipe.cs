﻿namespace Twitter.Clone.Trends.TrendsPipeline;

public class TrendsByContinentPipe(
    Action<HashtagRepository> next,
    IOptions<MakeTrendsSettings> makeTrendsSettings,
    TrendsByContinentRepository trendsByContinentRepository)
    : BasePipe(next)
{
    private readonly IOptions<MakeTrendsSettings> _makeTrendsSettings = makeTrendsSettings;
    private readonly TrendsByContinentRepository _trendsByContinentRepository = trendsByContinentRepository;

    public async override void Handle(HashtagRepository context)
    {
        var trends = (await context.GetHashtagsByTimeSpanAsync(_makeTrendsSettings.Value.ContinentTrendTimeSpan))
             .GroupBy(x => new { x.Name, x.Continent })
             .Select(g => new TrendsByContinent
             {
                 Name = g.Key.Name,
                 Continent = g.Key.Continent,
                 Count = g.Count()
             })
             .ToList();

        foreach (var trend in trends)
        {
            if (await _trendsByContinentRepository.TrendExistsAsync(trend.Name, trend.Continent))
                await _trendsByContinentRepository.UpdateAsync(trend.Name, trend.Continent, trend.Count);
            else
                await _trendsByContinentRepository.CreateAsync(trend);
        }
    }
}