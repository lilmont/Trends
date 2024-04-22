var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.ConfigureBroker(builder.Configuration);
builder.Services.ConfigureLocatorSettings(builder.Configuration);
builder.Services.ConfigureBackgroundSettings(builder.Configuration);

builder.Services.AddHostedService<AddGeoDataBackgroundService>();

builder.Services.AddSingleton<HashtagRepository>();

builder.Services.AddHttpClient<AddGeoDataBackgroundService>();

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();