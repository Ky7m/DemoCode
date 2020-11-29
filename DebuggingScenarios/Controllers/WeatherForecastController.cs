using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DebuggingScenarios.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            
            int[] vectorA = { 0, 2, 4, 5, 6 };
            int[] vectorB = { 1, 3, 5, 7, 8 };

            var dotProduct = vectorA.Combine(vectorB, (a, b) => a * b).Sum();
            if (dotProduct > 100 && rng.Next(1, 10) > 0)
            {
                if (Debugger.IsAttached)
                {
                    _ = dotProduct;
                }
                dotProduct = rng.Next(-10, -5);
                // Debug.Assert(dotProduct > 0);
                if (dotProduct < 0)
                {
                    Debugger.Launch();
                }
            }
            
            var weatherForecasts = Enumerable.Range(1, 30)
                .LogLinq("source", x => x.ToString())
                .Where(x => x > 10)
                .LogLinq("filtered", x=> x.ToString())
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                //.Take(5).LogLinq("limited", x=> x.Summary)
                .ToList();

            void AddExtraDay()
            {
                var extraDay = new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(31),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
                weatherForecasts.Add(extraDay);
            }
            
            AddExtraDay();
            
            return weatherForecasts;
        }
    }

    public static class SequenceExtensions
    {
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, T> func)
        {
            using (IEnumerator<T> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(e1.Current, e2.Current);
                }
            }
        }
        
        //[Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static IEnumerable<T> LogLinq<T>(this IEnumerable<T> enumerable, string logName, Func<T, string> printMethod)
        {
#if DEBUG
            int count = 0;
            foreach (var item in enumerable)
            {
                if (printMethod != null)
                {
                    Debug.WriteLine($"{logName}|item {count} = {printMethod(item)}");
                }
                count++;
                yield return item;
            }
            Debug.WriteLine($"{logName}|count = {count}");
#else   
    return enumerable;
#endif
        }
    }
}
