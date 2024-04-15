namespace Twitter.Clone.Trends.Services;

public interface IHashtagService
{
    Task MakeHashtagTrend(string hashtagId, string hashtagName, DateTime dateCreated, string ipAddress);
}

public class HashtagService : IHashtagService
{
    private readonly HashtagRepository _repository;

    public HashtagService (HashtagRepository repository)
    {
        _repository = repository;
    }
    public async Task MakeHashtagTrend(string hashtagId, string hashtagName, DateTime date, string ip)
    {
        var hashtags = new List<Hashtag>();
        var hashtagLogs = new List<HashtagLog>();
        var hashtagLog = new HashtagLog();

        var existingHashtag = await _repository.GetHashtagByNameAsync(hashtagName);

        string id;

        if (existingHashtag is null) 
        {
            await _repository.CreateHashtagAsync(new Hashtag
            {
                Name = hashtagName,
                Id = hashtagId,

            });

            id = hashtagId;
        }
        else id = existingHashtag.Id;

        await _repository.CreateLogAsync(new HashtagLog
        {
            HashtagId = id,
            DateCreated = date,
            IPAddress = ip,
        });

        var hashtagCount = from log in hashtagLogs
                           where date < log.DateCreated  
                           orderby hashtagLogs.Count() ascending 
                           select log;

    }
}
