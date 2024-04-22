namespace Twitter.Clone.Trends.Repositories;

public class HashtagRepository
{
    private readonly IMongoCollection<Hashtag> _hashtagsCollection;
    private readonly static InsertOneOptions _insertOneOptions = new();

    public HashtagRepository(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _hashtagsCollection = mongoDatabase.GetCollection<Hashtag>(
            Hashtag.CollectionName);
    }

    public async Task CreateAsync(Hashtag newHashtag, CancellationToken cancellationToken) =>
        await _hashtagsCollection.InsertOneAsync(newHashtag, _insertOneOptions, cancellationToken);

    public async Task<List<Hashtag>> GetHashtagsWithoutGeoInfoAsync(CancellationToken cancellationToken) =>
        await _hashtagsCollection
        .Find(
            p => string.IsNullOrEmpty(p.Country) ||
            string.IsNullOrEmpty(p.Continent)
            )
        .ToListAsync(cancellationToken);

    public async Task UpdateGeoInfoAsync(string id, string country, string continent, CancellationToken cancellationToken) =>
        await _hashtagsCollection.UpdateOneAsync(p => p.Id == id, Builders<Hashtag>.Update
            .Set(p => p.Country, country)
            .Set(p => p.Continent, continent));
}
