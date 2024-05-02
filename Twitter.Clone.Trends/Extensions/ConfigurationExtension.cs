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

    public static IServiceCollection ConfigureLocatorSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LocatorServiceSettings>(
             configuration.GetSection(LocatorServiceSettings.SectionName));

        return services;
    }

    public static IServiceCollection ConfigureBackgroundSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings.InboxBackgroundServiceSettings>(
             configuration.GetSection(AppSettings.InboxBackgroundServiceSettings.SectionName));

        return services;
    }
}
