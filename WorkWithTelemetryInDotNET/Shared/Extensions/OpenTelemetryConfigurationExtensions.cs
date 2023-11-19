using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Instrumentation.SqlClient;
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
        services.Configure<AspNetCoreInstrumentationOptions>(options =>
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

        services.Configure<HttpClientInstrumentationOptions>(options =>
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
        
        services.Configure<SqlClientInstrumentationOptions>(options =>
        {
            options.RecordException = true;
            options.SetDbStatementForText = true;
            options.SetDbStatementForStoredProcedure = true;
        });
        
        services.Configure<MassTransit.Monitoring.InstrumentationOptions>(_ =>
        {
        });

        services
            .AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .WithTracing(builder =>
            {
                builder
                    .AddSource(environment.ApplicationName, MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation();
                
                if (environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    builder.SetSampler(new AlwaysOnSampler());
                }
                
                builder.AddOtlpExporter();
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

                builder.AddOtlpExporter();
            });
            
            // services.AddLogging(builder =>
            // {
            //     builder.AddOpenTelemetry(options =>
            //     {
            //         var resourceBuilder = ResourceBuilder.CreateDefault();
            //         ConfigureResource(resourceBuilder);
            //         options.SetResourceBuilder(resourceBuilder);
            //         
            //         options.IncludeFormattedMessage = true;
            //         options.ParseStateValues = true;
            //         options.IncludeScopes = true;
            //
            //         options.AddOtlpExporter();
            //     });
            // });

        return services;

        void ConfigureResource(ResourceBuilder builder) =>
            builder.AddService(serviceName: environment.ApplicationName,
                    serviceVersion: typeof(OpenTelemetryConfigurationExtensions).Assembly.GetName().Version?.ToString() ?? "unknown",
                    serviceInstanceId: Environment.MachineName)
                .AddTelemetrySdk();
    }
}