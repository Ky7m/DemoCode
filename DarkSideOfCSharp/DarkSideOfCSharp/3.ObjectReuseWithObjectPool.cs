using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;
using Xunit;

namespace DarkSideOfCSharp
{
    public class ObjectReuseWithObjectPool
    {
        private readonly ObjectPool<StringBuilder> _stringBuilderPool;

        public ObjectReuseWithObjectPool()
        {
            _stringBuilderPool = ObjectPool.Create(new StringBuilderPooledObjectPolicy());
        }
        
        [Fact]
        public void AssertThatAllMethodsProduceSameResult()
        {
            var expected = CreateNewStringBuilder();
            Assert.Equal(expected,GetStringBuilderFromPool());
        }
        
        [Benchmark(Baseline = true)]
        public string CreateNewStringBuilder()
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }
        
        [Benchmark]
        public string GetStringBuilderFromPool()
        {
            // Request a StringBuilder from the pool.
            var stringBuilder = _stringBuilderPool.Get();

            try
            {
                return stringBuilder.ToString();
            }
            finally // Ensure this runs even if the main code throws.
            {
                // Return the StringBuilder to the pool.
                // There's no requirement that you return every object.
                // If you don't return an object, it will be garbage collected.
                _stringBuilderPool.Return(stringBuilder); 
            }
        }
    }
}