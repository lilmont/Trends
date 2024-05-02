namespace Twitter.Clone.Trends.Repositories;

public class InboxHashtagRepository
{
    private readonly IMongoCollection<Inbox> _inboxesCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();

    public InboxHashtagRepository(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _inboxesCollection = mongoDatabase.GetCollection<Inbox>(
            Inbox.CollectionName);
    }

    public async Task CreateAsync(Inbox newInbox, CancellationToken cancellationToken) =>
        await _inboxesCollection.InsertOneAsync(newInbox, _insertOneOptions, cancellationToken);

    public async Task<List<Inbox>> GetUnprocessedInboxAsync(CancellationToken cancellationToken) =>
        await _inboxesCollection.Find(p => p.IsProcessed == false).ToListAsync(cancellationToken);

    public async Task UpdateProcessedStatusAsync(string id, CancellationToken cancellationToken) =>
        await _inboxesCollection.UpdateOneAsync(p => p.Id == id, Builders<Inbox>.Update.Set(p => p.IsProcessed, true));
}
