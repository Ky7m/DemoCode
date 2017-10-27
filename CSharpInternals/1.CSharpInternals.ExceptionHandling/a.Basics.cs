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
        
        // Fact - ToString allocates a lot, especially with AggregateException
        // http://referencesource.microsoft.com/#mscorlib/system/AggregateException.cs,448

        private static void ThrowsNullReferenceException() => throw null;
    }
}