

namespace Twitter.Clone.Trends.Services;

public class HashtagRepository
{
    private readonly IMongoCollection<Hashtag> _hashtagsCollection;
    private readonly IMongoCollection<HashtagLog> _hashtagLogsCollection;

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
        _hashtagLogsCollection = mongoDatabase.GetCollection<HashtagLog>(
            HashtagLog.CollectionName);
    }

    public async Task CreateHashtagAsync(Hashtag newHashtag, CancellationToken cancellationToken) =>
        await _hashtagsCollection.InsertOneAsync(newHashtag, _insertOneOptions, cancellationToken);
    public async Task<List<Hashtag>> GetAllHashtagsAsync() =>
        await _hashtagsCollection.Find(_ => true).ToListAsync();
    public async Task<Hashtag> GetHashtagByIdAsync(string id) =>
        await _hashtagsCollection.Find(hashtag  => hashtag.Id == id).FirstOrDefaultAsync();
    public async Task<Hashtag> GetHashtagByNameAsync(string name) =>
        await _hashtagsCollection.Find(hashtag => hashtag.Name == name).FirstOrDefaultAsync();


    public async Task CreateLogAsync(HashtagLog newLog) =>
        await _hashtagLogsCollection.InsertOneAsync(newLog);
    public async Task<List<HashtagLog>> GetAllLogsAsync() =>
        await _hashtagLogsCollection.Find(_ => true).ToListAsync();
    public async Task<HashtagLog> GetLogByIdAsync(string id) =>
        await _hashtagLogsCollection.Find(hashtag => hashtag.Id == id).FirstOrDefaultAsync();
}
