using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class ReturnAwaitOrReturnTask
    {
        [Benchmark(Baseline = true)] 
        public async Task DirectGet()
        {
            var result = await GetData();
        }
        
        [Benchmark] 
        public async Task ViaAsync()
        {
            var result = await Async();
        }
        
        [Benchmark] 
        public async Task ViaSync()
        {
            var result = await Sync();
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