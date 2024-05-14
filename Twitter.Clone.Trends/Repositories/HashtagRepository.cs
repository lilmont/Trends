namespace Twitter.Clone.Trends.Repositories;

public class HashtagRepository(IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task CreateAsync(Hashtag newHashtag, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
            await dbContext.Hashtags.AddAsync(newHashtag, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<Hashtag>> GetHashtagsByTimeSpanAsync(int timeSpanInDays)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
            return await dbContext.Hashtags
                .Where(p => p.DateCreated >= DateTime.UtcNow.AddDays(-timeSpanInDays))
                .ToListAsync();
        }
    }
}
