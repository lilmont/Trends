namespace Twitter.Clone.Trends.Extensions;

public static class ConfigurationExtension
{
    public static IServiceCollection ConfigureBroker(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(typeof(IAssemblyMaker).Assembly);

            configure.UsingRabbitMq((context, cfg) =>
            {
                var settings = configuration.GetSection("EventBrokerConfiguration")
                .Get<RabbitMQSetting>()!;

                cfg.UseRawJsonDeserializer();

                cfg.Host(settings.Host, hostConfigure =>
                {
                    hostConfigure.Username(settings.Username);
                    hostConfigure.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(TrendsDatabaseSettings.SectionName)
                .Get<TrendsDatabaseSettings>();

        services.AddDbContext<TrendsDbContext>(options =>
        {
            if (settings is null)
            {
                throw new Exception("Invalid settings!");
            }
            options.UseMongoDB(settings.Host, settings.DatabaseName);
            
        });
        return services;
    }

    public static IServiceCollection ConfigureLocatorSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LocatorServiceSettings>(
             configuration.GetSection(LocatorServiceSettings.SectionName));

        return services;
    }

    public static IServiceCollection ConfigureBackgroundSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InboxBackgroundServiceSettings>(
             configuration.GetSection(InboxBackgroundServiceSettings.SectionName));

        return services;
    }

    public static IServiceCollection ConfigureMakeTrendsBackgroundServiceSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MakeTrendsBackgroundServiceSettings>(
             configuration.GetSection(MakeTrendsBackgroundServiceSettings.SectionName));

        return services;
    }

    public static IServiceCollection ConfigureMakeTrendsSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MakeTrendsSettings>(
             configuration.GetSection(MakeTrendsSettings.SectionName));

        return services;
    }
}
