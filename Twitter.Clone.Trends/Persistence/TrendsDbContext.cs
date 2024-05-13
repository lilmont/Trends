namespace Twitter.Clone.Trends.Persistence;

public class TrendsDbContext : DbContext
{
    public DbSet<Hashtag> Hashtags { get; set; }
    public DbSet<Inbox> Inboxes { get; set; }

    public TrendsDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hashtag>().ToCollection(Hashtag.CollectionName);
        modelBuilder.Entity<Inbox>().ToCollection(Inbox.CollectionName);
    }
}
