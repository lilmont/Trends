namespace Twitter.Clone.Trends.Repositories;

public class TrendsByContinentRepository(TrendsDbContext dbContext)
{
    private readonly TrendsDbContext _dbContext = dbContext;

    public async Task CreateAsync(TrendByContinent newTrend, CancellationToken cancellationToken)
    {
        await _dbContext.TrendsByContinent.AddAsync(newTrend, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TrendExistsAsync(string name, string continent, CancellationToken cancellationToken) =>
        await _dbContext.TrendsByContinent.Where(p => p.Name == name && p.Continent == continent)
        .AnyAsync(cancellationToken);

    public async Task UpdateAsync(string name, string continent, int count, CancellationToken cancellationToken)
    {
        var currentTrend = await _dbContext.TrendsByContinent
            .SingleOrDefaultAsync(p => p.Name == name && p.Continent == continent, cancellationToken);
        if (currentTrend is not null)
        {
            currentTrend.Count = count;
            _dbContext.Entry(currentTrend).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
