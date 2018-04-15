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
                        typeof(ValueTaskVsTaskRecommendedScenario),
                        typeof(ReturnAwaitOrReturnTask),
                    }
                ).Run(args, new BenchmarkConfig());
    }
}