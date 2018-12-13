using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HolidayPartySF2018AspNetCore22.HealthChecks
{
    public static class GCInfoHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddGCInfoCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
        {
            // Register a check of type GCInfo
            builder.AddCheck<GCInfoHealthCheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

            // Configure named options to pass the threshold into the check.
            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<GCInfoOptions>(name, options =>
                {
                    options.Threshold = thresholdInBytes.Value;
                });
            }

            return builder;
        }
    }
}