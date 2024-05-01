using Twitter.Clone.Trends.Extensions;
using Twitter.Clone.Trends.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.ConfigureBroker(builder.Configuration);

builder.Services.AddSingleton<HashtagRepository>();
builder.Services.AddSingleton<IHashtagService, HashtagService>();

var app = builder.Build();


app.UseHttpsRedirection();


app.Run();