using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class TaskFromResultOrCompletedTask
    {
        [Benchmark] 
        public Task TaskFromResult() => Task.FromResult(0);
        
        [Benchmark] 
        public Task TaskCompletedTask() => Task.CompletedTask;
    }
}