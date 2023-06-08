using System;
using Azure.Monitor.OpenTelemetry.Exporter;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Extensions.AzureMonitor;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Instrumentation.SqlClient;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Extensions;

[PublicAPI]
public static class OpenTelemetryConfigurationExtensions
{
    public static IServiceCollection AddOpenTelemetrySharedConfiguration(this IServiceCollection services, IConfiguration configuration, string applicationName)
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
                // filter app insights
                if (string.Equals(req.RequestUri?.Host, "dc.services.visualstudio.com", StringComparison.OrdinalIgnoreCase)
                    ||
                    req.RequestUri?.Host.Contains("applicationinsights.azure.com", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return false;
                }
            
                if (string.Equals(req.RequestUri?.Host, "localhost", StringComparison.OrdinalIgnoreCase))
                {
                    // filter log request to SEQ
                    return req.RequestUri?.Port != 5341;
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

        Action<ResourceBuilder> configureResource = builder => builder.AddService(
                serviceName: applicationName,
                serviceVersion: typeof(OpenTelemetryConfigurationExtensions).Assembly.GetName().Version
                    ?.ToString() ?? "unknown",
                serviceInstanceId: Environment.MachineName)
            .AddTelemetrySdk();
        services
            .AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(builder =>
            {
                builder
                    .AddSource(applicationName, MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
                    .SetSampler(new AlwaysOffSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation();
                var appiConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");
                if (!string.IsNullOrEmpty(appiConnectionString))
                {
                    builder
                        .SetSampler(sp =>
                        {
                            var options = sp.GetRequiredService<IOptionsMonitor<ApplicationInsightsSamplerOptions>>()
                                .Get(Options.DefaultName);
                            return new ApplicationInsightsSampler(options);
                        })
                        .AddAzureMonitorTraceExporter(x => x.ConnectionString = appiConnectionString);
                }

                builder.AddJaegerExporter();
                builder.AddOtlpExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName)
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
                var appiConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");
                if (!string.IsNullOrEmpty(appiConnectionString))
                {
                    builder.AddAzureMonitorMetricExporter(x => x.ConnectionString = appiConnectionString);
                }

                builder.AddOtlpExporter();
            });
            
            services.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    var resourceBuilder = ResourceBuilder.CreateDefault();
                    configureResource(resourceBuilder);
                    options.SetResourceBuilder(resourceBuilder);
                    
                    options.IncludeFormattedMessage = true;
                    options.ParseStateValues = true;
                    options.IncludeScopes = true;
                });
            });

        return services;
    }
}