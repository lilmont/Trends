namespace Twitter.Clone.Trends;

public class Endpoints(TrendsGlobalRepository trendsGlobalRepository)
{
    private readonly TrendsGlobalRepository _trendsGlobalRepository = trendsGlobalRepository;
    public void GetTopTenTrendsGlobally(RouteGroupBuilder mapGroup)
    {
        mapGroup.MapGet("/global", async () =>
        {
            return Results.Ok(await _trendsGlobalRepository.GetTopTenAsync());
        });
    }
}
