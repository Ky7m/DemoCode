using BenchmarkDotNet.Running;

namespace CSharpInternals.Undocumented
{
    static class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<KeywordsBenchmarks>();
        }
    }
}
