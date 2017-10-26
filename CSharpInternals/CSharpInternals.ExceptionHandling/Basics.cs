using System;
using Xunit;

namespace CSharpInternals.ExceptionHandling
{
    public class Basics
    {
        [Fact]
        public void Test()
        {
            Assert.Throws<NullReferenceException>(() => ThrowsNullReferenceException());
        }

        private static void ThrowsNullReferenceException() => throw null;
    }
}