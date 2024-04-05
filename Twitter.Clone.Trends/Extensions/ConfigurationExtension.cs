﻿namespace Twitter.Clone.Trends.Extensions;

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
}
