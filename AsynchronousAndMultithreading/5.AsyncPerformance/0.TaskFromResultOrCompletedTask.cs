using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AsyncPerformance
{
    public class TaskFromResultOrCompletedTask
    {
        [Benchmark(Baseline = true)] 
        public Task CompletedTask() => Task.CompletedTask;
        
        [Benchmark] 
        public Task TaskFromResult() => Task.FromResult(0);
    }
}