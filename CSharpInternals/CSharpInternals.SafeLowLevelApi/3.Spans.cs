using System;
using JetBrains.Annotations;
using Xunit;

namespace CSharpInternals.SafeLowLevelApi
{
    [UsedImplicitly]
    public class Spans
    {
        [Fact]
        public static void Test()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            ReadOnlySpan<int> span = new ReadOnlySpan<int>(a).Slice(6);
            Assert.Equal(4, span.Length);
        }
    }
}