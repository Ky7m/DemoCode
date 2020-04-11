using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CSharp8
{
    public class AsynchronousStreams
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public AsynchronousStreams(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static async IAsyncEnumerable<int> GenerateSequence([EnumeratorCancellation] CancellationToken token = default)
        {
            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(100, token);
                yield return i;
            }
        }

        [Fact]
        public async Task ConsumeSequenceAsync()
        {
            await foreach (var number in GenerateSequence()
                .WithCancellation(CancellationToken.None)
                .ConfigureAwait(false))
            {
                _testOutputHelper.WriteLine(number.ToString());
            }
        }
    }
}