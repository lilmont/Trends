namespace Twitter.Clone.Trends.Repositories;

public class InboxHashtagRepository(TrendsDbContext dbContext)
{
    private readonly TrendsDbContext _dbContext = dbContext;

    public async Task CreateAsync(Inbox newInbox, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(newInbox, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Inbox>> GetUnprocessedInboxAsync(CancellationToken cancellationToken) =>
        await _dbContext.Inboxes.Where(p => p.IsProcessed == false).ToListAsync(cancellationToken);

    public async Task UpdateProcessedStatusAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var currentInbox = await _dbContext.Inboxes.FindAsync(id, cancellationToken);
        if (currentInbox is not null)
        {
            currentInbox.IsProcessed = true;
            _dbContext.Entry(currentInbox).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
