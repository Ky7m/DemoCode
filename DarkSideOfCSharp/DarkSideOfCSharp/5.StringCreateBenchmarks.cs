using System.Text;
using BenchmarkDotNet.Attributes;
using Xunit;

namespace DarkSideOfCSharp
{
    public class StringCreateBenchmarks
    {
        private const int StringLength = 1_000_000;
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        
        [Fact]
        public void AssertThatAllMethodsProduceSameResult()
        {
            var expected = UsingStringBuilder();
            Assert.Equal(expected,UsingStringCreate());
        }
        
        [Benchmark(Baseline = true)]
        public string UsingStringBuilder()
        {
            var sb = new StringBuilder(StringLength);
            for (var i = 0; i < StringLength - 1; i++)
            {
                sb.Append('*');
            }

            sb.Append(Alphabet[7]);
            return sb.ToString();
        }
        
        [Benchmark]
        public string UsingStringCreate()
        {
            return string.Create(StringLength, Alphabet, (span, state) =>
            {
                for (var i = 0; i < span.Length - 1; i++)
                {
                    span[i] = '*';
                }

                span[^1] = state[7];
            });
        }
    }
}