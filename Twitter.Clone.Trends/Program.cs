var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumers(typeof(Program).Assembly);

    configure.UsingRabbitMq((context, cfg) =>
    {
        var settings = builder.Configuration.GetSection("EventBrokerConfiguration")
        .Get<EventBrokerSettings>()!;

        cfg.UseRawJsonDeserializer();

        cfg.Host(settings.Host, hostConfigure =>
        {
            hostConfigure.Username(settings.Username);
            hostConfigure.Password(settings.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<HashtagsService>();

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();