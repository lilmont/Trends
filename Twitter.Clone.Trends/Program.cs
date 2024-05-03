var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.ConfigureBroker(builder.Configuration);
builder.Services.ConfigureMakeTrendsBackgroundServiceSettings(builder.Configuration);
builder.Services.ConfigureMakeTrendsSettings(builder.Configuration);

builder.Services.AddSingleton<HashtagRepository>();
builder.Services.AddSingleton<TrendsByContinentRepository>();
builder.Services.AddSingleton<TrendsByCountryRepository>();
builder.Services.AddSingleton<TrendsGlobalRepository>();

builder.Services.AddHostedService<MakeTrendsBackgroundService>();

var app = builder.Build();


app.UseHttpsRedirection();


app.Run();