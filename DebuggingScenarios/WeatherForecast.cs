using System;
using System.Diagnostics;

// [assembly: DebuggerDisplay("Date: {Date,d,nq} | Summary: {Summary,nq}", Target = typeof(WeatherForecast))]
namespace DebuggingScenarios
{
    [DebuggerDisplay("Date: {" + nameof(Date) + ",nq} | Summary: {" + nameof(Summary) + ",nq}")]
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
