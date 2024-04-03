using MassTransit;
using Twitter.Clone.Trends.EventHandler;
using Twitter.Clone.Trends.Persistence;
using Twitter.Clone.Trends.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TrendsDatabaseSettings>(
    builder.Configuration.GetSection("TrendsDatabase"));

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<EventConsumer>();

    configure.UsingRabbitMq((context, rabbitmqConfigure) =>
    {
        var settings = builder.Configuration.GetSection("EventBrokerConfiguration")
        .Get<EventBrokerSettings>();

        rabbitmqConfigure.Host(settings!.Host, hostConfigure =>
        {
            hostConfigure.Username(settings.Username);
            hostConfigure.Password(settings.Password);
        });

        rabbitmqConfigure.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<HashtagsService>();

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();