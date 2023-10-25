using System;
using Azure.Monitor.OpenTelemetry.Exporter;
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
                // filter app insights
                if (string.Equals(req.RequestUri?.Host, "dc.services.visualstudio.com", StringComparison.OrdinalIgnoreCase)
                    ||
                    req.RequestUri?.Host.Contains("applicationinsights.azure.com", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return false;
                }
            
                if (string.Equals(req.RequestUri?.Host, "localhost", StringComparison.OrdinalIgnoreCase))
                {
                    // filter log request to SEQ or opentelemetry.proto.collector.logs
                    return req.RequestUri?.Port != 5341 && req.RequestUri?.Port != 4317;
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
                
                var appiConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");
                if (!string.IsNullOrEmpty(appiConnectionString))
                {
                    builder.AddAzureMonitorTraceExporter(x => x.ConnectionString = appiConnectionString);
                }
                
                builder.AddOtlpExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName)
                    .AddBuiltInMeters()
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
    
    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http");
}