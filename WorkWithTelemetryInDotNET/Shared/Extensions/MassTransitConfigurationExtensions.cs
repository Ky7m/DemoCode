using System;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;

namespace Shared.Extensions;

[PublicAPI]
public static class MassTransitConfigurationExtensions
{
    public static IServiceCollection AddMassTransitSharedConfiguration(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator> configureMassTransitBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> configureRabbitMqBus = null)
    {
        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();
            configureMassTransitBus?.Invoke(bus);
            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.UseMessageRetry(r => r.Immediate(3));

                configureRabbitMqBus?.Invoke(context, cfg);
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}