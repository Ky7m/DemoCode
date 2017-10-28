using System.Collections.Generic;
using Xunit;

namespace CSharpInternals.Strings
{
    public class StringInterning 
    {
        [Fact]
        public void Basics()
        {
            var str1 = "string";
            var str2 = "string";
            
            Assert.True(ReferenceEquals(str1, str2));
        }
        
        [Fact]
        public void MoreComplex()
        {
            var str1 = "string";
            var str2 = "str";
            var str3 = str2 + "ing";
            var str4 = GetFromInput();
            
            Assert.False(ReferenceEquals(str1, str3));
            Assert.False(ReferenceEquals(str1, str4));
        }
        
        // https://github.com/Microsoft/msbuild/blob/master/src/Shared/OpportunisticIntern.cs#L770,L1020
        [Fact]
        public void UsingClrPool()
        {
            var str1 = "string";
            var str2 = "str";
            var str3 = string.Intern(str2 + "ing");
            var str4 = string.Intern(GetFromInput());
            
            Assert.True(ReferenceEquals(str1, str3));
            Assert.True(ReferenceEquals(str1, str4));
            Assert.True(ReferenceEquals(str3, str4));
        }
        
        [Fact]
        public void UsingLocalPool()
        {
            var localPool = new LocalPool();
            
            var str1 = localPool.GetOrCreate("string");
            var str2 = localPool.GetOrCreate("str");
            var str3 = localPool.GetOrCreate(str2 + "ing");
            var str4 = localPool.GetOrCreate(GetFromInput());
            
            Assert.True(ReferenceEquals(str1, str3));
            Assert.True(ReferenceEquals(str1, str4));
            Assert.True(ReferenceEquals(str3, str4));
        }

        private static string GetFromInput()
        {
            return new string(new[] {'s', 't', 'r', 'i', 'n', 'g'});
        }
            
        private class LocalPool
        {
            private readonly Dictionary<string,string> _stringPool = new Dictionary<string, string>();

            public string GetOrCreate(string entry)
            {
                if (_stringPool.TryGetValue(entry, out var result))
                {
                    return result;
                }
                
                _stringPool.Add(entry, entry);
                return _stringPool[entry];
            }
        }
    }
}