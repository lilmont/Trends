namespace Twitter.Clone.Trends.Repositories;

public class TrendsGlobalRepository
{
    private readonly IMongoCollection<TrendsGlobal> _trendsGlobalCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();
    private readonly static UpdateOptions _updateOptions = new();

    public TrendsGlobalRepository(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _trendsGlobalCollection = mongoDatabase.GetCollection<TrendsGlobal>(
            TrendsGlobal.CollectionName);
    }

    public async Task CreateAsync(TrendsGlobal newTrend, CancellationToken cancellationToken) =>
        await _trendsGlobalCollection.InsertOneAsync(newTrend, _insertOneOptions, cancellationToken);

    public async Task<bool> TrendExistsAsync(string name, CancellationToken cancellationToken)
    {
        return await _trendsGlobalCollection.Find(p => p.Name == name).AnyAsync(cancellationToken);
    }

    public async Task UpdateAsync(string name, int count, CancellationToken cancellationToken) =>
        await _trendsGlobalCollection
        .UpdateOneAsync(p => p.Name == name, Builders<TrendsGlobal>.Update.Set(p => p.Count, count), _updateOptions, cancellationToken);
}
