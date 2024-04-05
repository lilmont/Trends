namespace Twitter.Clone.Trends.Services;

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
}
