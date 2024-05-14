namespace Twitter.Clone.Trends.Repositories;

public class TrendsByCountryRepository(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task CreateAsync(TrendByCountry newTrend, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
            await dbContext.TrendsByCountry.AddAsync(newTrend, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> TrendExistsAsync(string name, string country, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
            return await dbContext.TrendsByCountry
                .Where(p => p.Name == name && p.Country == country)
                .AnyAsync(cancellationToken);
        }
    }


    public async Task UpdateAsync(string name, string country, int count, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
            var currentTrend = await dbContext.TrendsByCountry
            .SingleOrDefaultAsync(p => p.Name == name && p.Country == country, cancellationToken);
            if (currentTrend is not null)
            {
                currentTrend.Count = count;
                dbContext.Entry(currentTrend).State = EntityState.Modified;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
