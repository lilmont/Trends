namespace Twitter.Clone.Trends.Repositories;

public class TrendsGlobalRepository(TrendsDbContext dbContext)
{
    private readonly TrendsDbContext _dbContext = dbContext;

    public async Task CreateAsync(TrendGlobal newTrend, CancellationToken cancellationToken)
    {
        await _dbContext.TrendsGlobal.AddAsync(newTrend, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TrendExistsAsync(string name, CancellationToken cancellationToken) =>
        await _dbContext.TrendsGlobal.AnyAsync(p => p.Name == name, cancellationToken);

    public async Task UpdateAsync(string name, int count, CancellationToken cancellationToken)
    {
        var currentTrend = await _dbContext.TrendsGlobal.SingleOrDefaultAsync(p => p.Name == name, cancellationToken);
        if (currentTrend is not null)
        {
            currentTrend.Count = count;
            _dbContext.Entry(currentTrend).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
