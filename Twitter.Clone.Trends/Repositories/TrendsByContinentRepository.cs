namespace Twitter.Clone.Trends.Repositories;

public class TrendsByContinentRepository
{
    private readonly IMongoCollection<TrendsByContinent> _trendsByContinentCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();
    private readonly static UpdateOptions _updateOptions = new();

    public TrendsByContinentRepository(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _trendsByContinentCollection = mongoDatabase.GetCollection<TrendsByContinent>(
            TrendsByContinent.CollectionName);
    }

    public async Task CreateAsync(TrendsByContinent newTrend, CancellationToken cancellationToken) =>
        await _trendsByContinentCollection.InsertOneAsync(newTrend, _insertOneOptions, cancellationToken);

    public async Task<bool> TrendExistsAsync(string name, string continent, CancellationToken cancellationToken)
    {
        var filter = Builders<TrendsByContinent>.Filter.And(
        Builders<TrendsByContinent>.Filter.Eq(x => x.Name, name),
            Builders<TrendsByContinent>.Filter.Eq(x => x.Continent, continent));

        return await _trendsByContinentCollection.Find(filter).AnyAsync(cancellationToken);
    }

    public async Task UpdateAsync(string name, string continent, int count, CancellationToken cancellationToken) =>
        await _trendsByContinentCollection
        .UpdateOneAsync(p => p.Name == name && p.Continent == continent,
            Builders<TrendsByContinent>.Update.Set(p => p.Count, count), _updateOptions, cancellationToken);
}
