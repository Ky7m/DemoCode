using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CSharp8
{
    public class AsynchronousDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public AsynchronousDisposable(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        [Fact]
        public async Task AsyncDisposal()
        {
            // await using var resource = new AsyncResource(_testOutputHelper);
            await using (var resource = new AsyncResource(_testOutputHelper))
            {
                await resource.PerformWorkAsync();
            }
            _testOutputHelper.WriteLine("After the await using statement");
        }
    }

    public sealed class AsyncResource : IAsyncDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AsyncResource(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        public async ValueTask DisposeAsync()
        {
            _testOutputHelper.WriteLine("Disposing asynchronously...");
            await Task.Delay(2000);
            _testOutputHelper.WriteLine("... done");
        }

        public async Task PerformWorkAsync()
        {
            _testOutputHelper.WriteLine("Performing work asynchronously...");
            await Task.Delay(2000);
            _testOutputHelper.WriteLine("... done");
        }
    }
}