namespace HolidayPartySF2018AspNetCore22.HealthChecks
{
    public sealed class GCInfoOptions
    {
        // The failure threshold (in bytes)
        public long Threshold { get; set; } = 1024L * 1024L * 10L;
    }
}