using System.Threading;
using System.Threading.Tasks;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace MultithreadedAsync
{
    public class AsyncCodeGenAndThreads : BaseTestHelpersClass
    {
        private readonly ITestOutputHelper _output;

        public AsyncCodeGenAndThreads(ITestOutputHelper output) : base(output)
        {
            _output = output;
        }
        
        [Fact]
        public async Task AsyncRegularFlow()
        {
            _output.WriteLine($"{nameof(AsyncRegularFlow)}.before. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
            await DelayAsync();
            _output.WriteLine($"{nameof(AsyncRegularFlow)}.after. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
        }

        [Fact]
        public async Task AsyncFastFlow()
        {
            _output.WriteLine($"{nameof(AsyncFastFlow)}.before. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
            await CompletedAsync();
            _output.WriteLine($"{nameof(AsyncFastFlow)}.after. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
        }

        private async Task DelayAsync()
        {
            _output.WriteLine($"{nameof(DelayAsync)}.prologue. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(100);
            _output.WriteLine($"{nameof(DelayAsync)}.continuation. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
        }
        
        private async Task CompletedAsync()
        {
            _output.WriteLine($"{nameof(CompletedAsync)}.prologue. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
            await Task.CompletedTask;
            _output.WriteLine($"{nameof(CompletedAsync)}.continuation. ThreadId #{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}