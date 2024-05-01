namespace Twitter.Clone.Trends.Repositories;

public class TrendsByContinentRepository
{
    private readonly IMongoCollection<TrendsByContinent> _trendsByContinentCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();

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

    public async Task CreateAsync(TrendsByContinent newTrend) =>
        await _trendsByContinentCollection.InsertOneAsync(newTrend, _insertOneOptions);

    public async Task<bool> TrendExistsAsync(string name, string continent)
    {
        var filter = Builders<TrendsByContinent>.Filter.And(
        Builders<TrendsByContinent>.Filter.Eq(x => x.Name, name),
            Builders<TrendsByContinent>.Filter.Eq(x => x.Continent, continent)
        );

        return await _trendsByContinentCollection.Find(filter).AnyAsync();
    }

    public async Task UpdateAsync(string name, string continent, int count) =>
        await _trendsByContinentCollection
        .UpdateOneAsync(p => p.Name == name && p.Continent == continent,
            Builders<TrendsByContinent>.Update.Set(p => p.Count, count));
}
