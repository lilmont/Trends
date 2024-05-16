namespace Twitter.Clone.Trends.Repositories;

public class TrendsByContinentRepository(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task CreateAsync(TrendByContinent newTrend, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        await dbContext.TrendsByContinent.AddAsync(newTrend, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TrendExistsAsync(string name, string continent, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        return await dbContext.TrendsByContinent
            .Where(p => p.Name == name && p.Continent == continent)
            .AnyAsync(cancellationToken);
    }

    public async Task UpdateAsync(string name, string continent, int count, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        var currentTrend = await dbContext.TrendsByContinent
        .SingleOrDefaultAsync(p => p.Name == name && p.Continent == continent, cancellationToken);
        if (currentTrend is not null)
        {
            currentTrend.Count = count;
            dbContext.Entry(currentTrend).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<TrendsResponse>> GetTrendsByContinentAsync(string continentName)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        return await dbContext.TrendsByContinent.OrderByDescending(p => p.Count)
            .Where(p => p.Continent == continentName)
            .Select(p => new TrendsResponse(p.Name, p.Count))
            .ToListAsync();
    }
}
