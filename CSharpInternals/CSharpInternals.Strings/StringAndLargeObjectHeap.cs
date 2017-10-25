using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace CSharpInternals.Strings
{
    public class StringAndLargeObjectHeap
    {
        private readonly int _maxGeneration = GC.MaxGeneration;

        [Fact]
        public void CheckGCMaxGenerationValue()
        {
            Assert.Equal(2, _maxGeneration);
        }
        
        [Fact]
        public void IfUseStringType()
        {
            var str = GetRandomTextAsString();
            Assert.Equal(_maxGeneration, GC.GetGeneration(str)); // in LOH
        }
        
        [Fact]
        public void IfUseStringBuilderType()
        {
            var sb = GetRandomTextAsStringBuilder();
            
            Assert.NotEqual(_maxGeneration, GC.GetGeneration(sb)); // not in LOH
            Assert.Equal(0, GC.GetGeneration(sb));
        }

        private static string GetRandomTextAsString()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[85000];
                rng.GetBytes(bytes);
                return new string(bytes.Select(Convert.ToChar).ToArray());
            }
        }
        
        private static StringBuilder GetRandomTextAsStringBuilder()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[85000];
                rng.GetBytes(bytes);
                return new StringBuilder().Append(bytes.Select(Convert.ToChar).ToArray());
            }
        }
    }
}