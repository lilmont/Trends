namespace Twitter.Clone.Trends.Repositories;

public class TrendsByCountryRepository
{
    private readonly IMongoCollection<TrendsByCountry> _trendsByCountryCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();

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

    public async Task CreateAsync(TrendsByCountry newTrend) =>
        await _trendsByCountryCollection.InsertOneAsync(newTrend, _insertOneOptions);

    public async Task<bool> TrendExistsAsync(string name, string country)
    {
        var filter = Builders<TrendsByCountry>.Filter.And(
        Builders<TrendsByCountry>.Filter.Eq(x => x.Name, name),
            Builders<TrendsByCountry>.Filter.Eq(x => x.Country, country)
        );

        return await _trendsByCountryCollection.Find(filter).AnyAsync();
    }

    public async Task UpdateAsync(string name, string country, int count) =>
        await _trendsByCountryCollection
        .UpdateOneAsync(p => p.Name == name && p.Country == country, 
            Builders<TrendsByCountry>.Update.Set(p => p.Count, count));
}
