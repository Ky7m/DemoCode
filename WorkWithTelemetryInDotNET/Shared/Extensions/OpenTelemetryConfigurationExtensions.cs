using System;
using Azure.Monitor.OpenTelemetry.Exporter;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Extensions;

[PublicAPI]
public static class OpenTelemetryConfigurationExtensions
{
    public static IServiceCollection AddSharedOpenTelemetryTracing(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        return services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddSource(applicationName)
                .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService(applicationName)
                    .AddTelemetrySdk())
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter = req => !(
                        (req.Request.Path.Equals("/", StringComparison.OrdinalIgnoreCase) && req.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)) ||
                        req.Request.Path.Equals("/health", StringComparison.OrdinalIgnoreCase) ||
                        req.Request.Path.Equals("/robots933456.txt", StringComparison.OrdinalIgnoreCase) ||
                        req.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase));
                    options.RecordException = true;
                })
                .AddHttpClientInstrumentation(opts =>
                {
                    opts.SetHttpFlavor = false;
                    opts.RecordException = true;
                    opts.Filter = req =>
                    {
                        // filter app insights
                        if (string.Equals(req.RequestUri?.Host, "dc.services.visualstudio.com", StringComparison.OrdinalIgnoreCase))
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
                })
                .AddSqlClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.SetDbStatementForText = true;
                })
                .AddSource("NServiceBus.Extensions.Diagnostics");

            var appiConnectionString = configuration.GetValue<string>("ApplicationInsights:ConnectionString");
            if (!string.IsNullOrEmpty(appiConnectionString))
            {
                builder.AddAzureMonitorTraceExporter(x => x.ConnectionString = appiConnectionString);
            }

            builder.AddJaegerExporter();
        });
    }
}