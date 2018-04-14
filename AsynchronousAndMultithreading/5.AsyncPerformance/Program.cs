using BenchmarkDotNet.Running;

namespace AsyncPerformance
{
    class Program
    {
        static void Main(string[] args) =>
            BenchmarkSwitcher.FromTypes(new[]
                    {
                        typeof(TaskFromResultOrCompletedTask),
                        typeof(ReturnTaskOfT),
                        typeof(ReturnAwaitOrReturnTask),
                        typeof(ValueTaskVsTaskRecommendedScenario),
                    }
                ).Run(args, new BenchmarkConfig());
    }
}