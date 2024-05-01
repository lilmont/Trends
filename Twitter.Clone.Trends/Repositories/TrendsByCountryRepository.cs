namespace Twitter.Clone.Trends.Repositories;

public class TrendsByCountryRepository
{
    private readonly IMongoCollection<TrendsByCountry> _trendsByCountryCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();
    private readonly static UpdateOptions _updateOptions = new();

    public TrendsByCountryRepository(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _trendsByCountryCollection = mongoDatabase.GetCollection<TrendsByCountry>(
            TrendsByCountry.CollectionName);
    }

    public async Task CreateAsync(TrendsByCountry newTrend, CancellationToken cancellationToken) =>
        await _trendsByCountryCollection.InsertOneAsync(newTrend, _insertOneOptions, cancellationToken);

    public async Task<bool> TrendExistsAsync(string name, string country, CancellationToken cancellationToken)
    {
        var filter = Builders<TrendsByCountry>.Filter.And(
        Builders<TrendsByCountry>.Filter.Eq(x => x.Name, name),
            Builders<TrendsByCountry>.Filter.Eq(x => x.Country, country)
        );

        return await _trendsByCountryCollection.Find(filter).AnyAsync(cancellationToken);
    }

    public async Task UpdateAsync(string name, string country, int count, CancellationToken cancellationToken) =>
        await _trendsByCountryCollection
        .UpdateOneAsync(p => p.Name == name && p.Country == country,
            Builders<TrendsByCountry>.Update.Set(p => p.Count, count), _updateOptions, cancellationToken);
}
