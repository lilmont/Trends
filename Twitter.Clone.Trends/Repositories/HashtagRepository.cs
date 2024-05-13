using MongoDB.Driver;

namespace Twitter.Clone.Trends.Repositories;

public class HashtagRepository(TrendsDbContext dbContext)
{
    private readonly TrendsDbContext _dbContext = dbContext;

    public async Task CreateAsync(Hashtag newHashtag, CancellationToken cancellationToken)
    {
        await _dbContext.Hashtags.AddAsync(newHashtag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Hashtag>> GetHashtagsByTimeSpanAsync(int timeSpanInDays) =>
        await _dbContext.Hashtags.Where(p => p.DateCreated <= DateTime.UtcNow.AddDays(-timeSpanInDays)).ToListAsync();
}
