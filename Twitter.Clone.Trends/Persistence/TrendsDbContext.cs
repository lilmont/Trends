namespace Twitter.Clone.Trends.Persistence;

public class TrendsDbContext : DbContext
{
    public DbSet<Hashtag> Hashtags { get; set; }
    public DbSet<Inbox> Inboxes { get; set; }
    public DbSet<TrendByContinent> TrendsByContinent { get; set; }
    public DbSet<TrendByCountry> TrendsByCountry { get; set; }
    public DbSet<TrendGlobal> TrendsGlobal { get; set; }

    public TrendsDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hashtag>().ToCollection(Hashtag.CollectionName);
        modelBuilder.Entity<Inbox>().ToCollection(Inbox.CollectionName);
        modelBuilder.Entity<TrendByContinent>().ToCollection(TrendByContinent.CollectionName);
        modelBuilder.Entity<TrendByCountry>().ToCollection(TrendByCountry.CollectionName);
        modelBuilder.Entity<TrendGlobal>().ToCollection(TrendGlobal.CollectionName);
    }
}
