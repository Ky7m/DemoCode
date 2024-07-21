using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared.Metrics;

namespace Shared.Extensions;

[PublicAPI]
public static class OpenTelemetryConfigurationExtensions
{
    public static IServiceCollection AddOpenTelemetryDefaults(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
        {
            options.Filter = httpContext =>
            {
                var request = httpContext.Request;
                return !(
                    (request.Path.Equals("/", StringComparison.OrdinalIgnoreCase) &&
                     request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)) ||
                    request.Path.Equals("/health", StringComparison.OrdinalIgnoreCase) ||
                    request.Path.Equals("/metrics", StringComparison.OrdinalIgnoreCase) ||
                    request.Path.Equals("/robots933456.txt", StringComparison.OrdinalIgnoreCase) ||
                    request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase));
            };
            options.RecordException = true;
        });

        services.Configure<HttpClientTraceInstrumentationOptions>(options =>
        {
            options.FilterHttpRequestMessage = req =>
            {
                if (string.Equals(req.RequestUri?.Host, "localhost", StringComparison.OrdinalIgnoreCase))
                {
                    // filter log request to opentelemetry.proto.collector.logs
                    return req.RequestUri?.Port != 4317;
                }
            
                return true;
            };
            options.RecordException = true;
        });
        
        services.Configure<MassTransit.Monitoring.InstrumentationOptions>(_ =>
        {
        });

        services.AddLogging(builder =>
        {
            builder.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });
        });

        var serviceName = environment.ApplicationName;
        services
            .AddOpenTelemetry()
            .UseOtlpExporter()
            .ConfigureResource(builder => builder.AddService(serviceName))
            .WithTracing(builder =>
            {
                builder
                    .AddSource(serviceName, MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
                
                if (environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    builder.SetSampler(new AlwaysOnSampler());
                }
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName)
                    .AddMeter(OrderMetrics.MeterName)
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        return services;
    }
}