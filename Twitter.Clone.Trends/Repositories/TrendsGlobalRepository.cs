namespace Twitter.Clone.Trends.Repositories;

public class TrendsGlobalRepository(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task CreateAsync(TrendGlobal newTrend, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        await dbContext.TrendsGlobal.AddAsync(newTrend, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TrendExistsAsync(string name, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        return await dbContext.TrendsGlobal.AnyAsync(p => p.Name == name, cancellationToken);
    }

    public async Task UpdateAsync(string name, int count, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        var currentTrend = await dbContext.TrendsGlobal.SingleOrDefaultAsync(p => p.Name == name, cancellationToken);
        if (currentTrend is not null)
        {
            currentTrend.Count = count;
            dbContext.Entry(currentTrend).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
