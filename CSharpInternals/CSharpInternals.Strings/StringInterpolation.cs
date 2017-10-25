using System;
using System.Net;
using System.Threading.Tasks;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Strings
{
    public class StringInterpolation : BaseTestHelpersClass
    {
        public StringInterpolation(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void WholeProgramInString()
        {
            WriteLine($@"{((Func<Task<string>>) (async () =>
            {
                WriteLine("Hello from there! Dowloading information...");
                return await new WebClient().DownloadStringTaskAsync("https://www.ifesenko.com/robots.txt");
            }))().GetAwaiter().GetResult()}!");
        }
    }
}