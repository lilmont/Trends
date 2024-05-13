namespace Twitter.Clone.Trends.Repositories;

public class TrendsByCountryRepository(TrendsDbContext dbContext)
{
    private readonly TrendsDbContext _dbContext = dbContext;

    public async Task CreateAsync(TrendByCountry newTrend, CancellationToken cancellationToken)
    {
        await _dbContext.TrendsByCountry.AddAsync(newTrend, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TrendExistsAsync(string name, string country, CancellationToken cancellationToken) =>
        await _dbContext.TrendsByCountry.Where(p => p.Name == name && p.Country == country)
        .AnyAsync(cancellationToken);

    public async Task UpdateAsync(string name, string country, int count, CancellationToken cancellationToken)
    {
        var currentTrend = await _dbContext.TrendsByCountry
            .SingleOrDefaultAsync(p => p.Name == name && p.Country == country, cancellationToken);
        if (currentTrend is not null)
        {
            currentTrend.Count = count;
            _dbContext.Entry(currentTrend).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
