using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class ReturnTaskOfT
    {
        private readonly Task<int> _cachedTask = Task.FromResult(0);
        
        [Benchmark(Baseline = true)] 
        public Task<int> CachedTask() => _cachedTask;
        
        [Benchmark] 
        public Task<int> TaskCompletionSource()
        {
            var tsc = new TaskCompletionSource<int>();
            tsc.SetResult(0);
            return tsc.Task;
        }
        
        [Benchmark] 
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Task<int> TaskFromResult() => Task.FromResult(0);
        
        [Benchmark] 
        public async Task<int> TaskWithAsyncKeyword() => 0;
        
        [Benchmark] 
        public ValueTask<int> ValueTask() => new ValueTask<int>(0);
    }
}