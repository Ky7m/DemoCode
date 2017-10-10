using BenchmarkDotNet.Running;

namespace CSharpInternals
{
    internal static class Scratchpad
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(UndocumentedKeywordsBenchmarks)
            });
            switcher.Run(args);
        }
    }
}