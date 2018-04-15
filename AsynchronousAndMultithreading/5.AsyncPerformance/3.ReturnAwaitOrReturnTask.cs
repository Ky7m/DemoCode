using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class ReturnAwaitOrReturnTask
    {
        [Benchmark(Baseline = true)] 
        public async Task<string> DirectGet()
        {
            return await GetData();
        }
        
        [Benchmark] 
        public async Task<string> ViaAsync()
        {
            return await Async();
        }
        
        [Benchmark] 
        public async Task<string> ViaSync()
        {
            return await Sync();
        }

        private static async Task<string> Async()
        {
            return await GetData();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Task<string> Sync()
        {
            return GetData();
        }
        
        private static async Task<string> GetData()
        {
            await Task.CompletedTask;
            return string.Empty;
        }
    }
}