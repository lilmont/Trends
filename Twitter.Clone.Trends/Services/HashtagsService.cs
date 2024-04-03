using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Twitter.Clone.Trends.Models.Entities;
using Twitter.Clone.Trends.Persistence;

namespace Twitter.Clone.Trends.Services;

public class HashtagsService
{
    private readonly IMongoCollection<Hashtag> _hashtagsCollection;

    public HashtagsService(
        IOptions<TrendsDatabaseSettings> trendsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            trendsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trendsDatabaseSettings.Value.DatabaseName);

        _hashtagsCollection = mongoDatabase.GetCollection<Hashtag>(
            trendsDatabaseSettings.Value.HashtagsCollectionName);
    }

    public async Task CreateAsync(Hashtag newHashtag) =>
        await _hashtagsCollection.InsertOneAsync(newHashtag);
}
