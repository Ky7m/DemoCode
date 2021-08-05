using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Xunit;

namespace DarkSideOfCSharp
{
    public class StringAndSpanBenchmarks
    {
        private const string FullName = "FirstName LastName";
        
        [Fact]
        public void AssertThatAllMethodsProduceSameResult()
        {
            var expected = GetLastName(FullName);
            Assert.Equal(expected,GetLastNameUsingSubstring(FullName));
            Assert.Equal(expected,GetLastNameWithSpan(FullName).ToString());
        }


        [Benchmark(Baseline = true), Arguments(FullName)]
        public string GetLastName(string fullName)
        {
            var names = fullName.Split(" ");
            var lastName = names.LastOrDefault();
            return lastName ?? string.Empty;
        }

        [Benchmark, Arguments(FullName)]
        public string GetLastNameUsingSubstring(string fullName)
        {
            var lastSpaceIndex = fullName.LastIndexOf(" ", StringComparison.OrdinalIgnoreCase);

            return lastSpaceIndex == -1
                ? string.Empty
                : fullName[(lastSpaceIndex + 1)..];
        }

        [Benchmark, Arguments(FullName)]
        public ReadOnlySpan<char> GetLastNameWithSpan(ReadOnlySpan<char> fullName)
        {
            var lastSpaceIndex = fullName.LastIndexOf(' ');

            return lastSpaceIndex == -1 
                ? ReadOnlySpan<char>.Empty 
                : fullName[(lastSpaceIndex + 1)..];
        }
    }
}