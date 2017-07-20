using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace CSharp7
{
    public sealed class GeneralizedAsyncReturnTypes
    {
        private static bool _cache;
        private static int _cacheResult;

        public GeneralizedAsyncReturnTypes()
        {
            RunExampleAsync().Wait();
        }

        public static async Task RunExampleAsync()
        {
            WriteLine($"Main Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            var result = await AsyncCall();
            WriteLine(result);
            WriteLine("-----------");
            WriteLine("2nd attempt");
            result = await AsyncCall();
            WriteLine(result);
        }

        private static ValueTask<int> AsyncCall()
        {
            WriteLine($"AsyncCall Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            return _cache ? new ValueTask<int>(_cacheResult) : new ValueTask<int>(LoadCache());
        }

        private static async Task<int> LoadCache()
        {
            WriteLine($"LoadCache. Cache miss. Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100);
            _cache = true;
            _cacheResult = 5;
            return _cacheResult;
        }
    }
}
