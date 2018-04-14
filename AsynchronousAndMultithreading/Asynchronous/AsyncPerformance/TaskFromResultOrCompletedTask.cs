using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class TaskFromResultOrCompletedTask
    {
        [Benchmark(Baseline = true)] 
        public Task TaskCompletedTask() => Task.CompletedTask;
        
        [Benchmark] 
        public Task TaskFromResult() => Task.FromResult(0);
    }
}