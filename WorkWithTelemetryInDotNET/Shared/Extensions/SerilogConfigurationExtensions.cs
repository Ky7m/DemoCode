using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.Refit.Destructurers;

namespace Shared.Extensions;

[PublicAPI]
public static class SerilogConfigurationExtensions
{
    public static IHostBuilder UseSharedSerilogConfiguration(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new IExceptionDestructurer[]
                    {
                        new DbUpdateExceptionDestructurer(),
                        new ApiExceptionDestructurer(destructureCommonExceptionProperties: false, destructureHttpContent: false)
                    }))
                .Enrich.WithMachineName()
                .Enrich.WithAssemblyVersion(true)
                .Enrich.WithCorrelationIdHeader("X-Correlation-ID");

            if (context.HostingEnvironment.IsDevelopment())
            {
                configuration
                    .MinimumLevel.Debug()
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Seq("http://localhost:5341");
            }
            else
            {
                configuration
                    .MinimumLevel.Warning()
                    .WriteTo.Async(x => x.Console(LogEventLevel.Error));
            }

            var appiConnectionString = context.Configuration.GetValue<string>("ApplicationInsights:ConnectionString");
            if (!string.IsNullOrEmpty(appiConnectionString))
            {
                var telemetryConfiguration = new TelemetryConfiguration
                {
                    ConnectionString = appiConnectionString
                };
                configuration.WriteTo.Async(x => x.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces, LogEventLevel.Warning));
            }

            var homeFolder = Environment.GetEnvironmentVariable("HOME");
            var isRunningInAzureWebApp = !string.IsNullOrEmpty(homeFolder) && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            if (isRunningInAzureWebApp)
            {
                configuration
                    .MinimumLevel.Information()
                    .WriteTo.Async(x => x.File(
                        Path.Combine(homeFolder, "LogFiles", "Application", "diagnostics.txt"),
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 10 * 1024 * 1024,
                        retainedFileCountLimit: 2,
                        rollOnFileSizeLimit: true,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(1)));
            }


            configuration.Enrich.WithMessageTemplate();
            configuration.Enrich.WithTraceIdAndSpanId();
            configuration.WriteTo.Async(x => x.OpenTelemetry(resourceAttributes: new Dictionary<string, object>
            {
                {"service.name", context.HostingEnvironment.ApplicationName},
                {"service.instance.id", Environment.MachineName}
            }));
        });
    }
    
    public static IApplicationBuilder UseSharedSerilogRequestLogging(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging(options =>
        {
            options.IncludeQueryInRequestPath = true;
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms.";
        });
    }
}