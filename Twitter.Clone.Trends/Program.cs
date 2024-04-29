using Twitter.Clone.Trends.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.ConfigureBroker(builder.Configuration);
builder.Services.ConfigureLocatorSettings(builder.Configuration);
builder.Services.ConfigureBackgroundSettings(builder.Configuration);

builder.Services.AddHostedService<InboxBackgroundService>();

builder.Services.AddSingleton<HashtagRepository>();
builder.Services.AddSingleton<InboxHashtagRepository>();
builder.Services.AddSingleton<InboxProcessor>();

builder.Services.AddHttpClient<InboxBackgroundService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();