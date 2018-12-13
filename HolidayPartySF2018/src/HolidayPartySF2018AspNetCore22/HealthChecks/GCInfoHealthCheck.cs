using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HolidayPartySF2018AspNetCore22.HealthChecks
{
    public class GCInfoHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<GCInfoOptions> _options;

        public GCInfoHealthCheck(IOptionsMonitor<GCInfoOptions> options)
        {
            _options = options;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = _options.Get(context.Registration.Name);

            // It will report degraded status if the application is using
            // more than the configured amount of memory (10mb by default).
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };

            // Report failure if the allocated memory is >= the threshold.
            var result = allocated >= options.Threshold ? context.Registration.FailureStatus : HealthStatus.Healthy;

            return Task.FromResult(new HealthCheckResult(
                result,
                description: $"reports degraded status if allocated bytes >= {options.Threshold}",
                data: data));
        }
    }
}