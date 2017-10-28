using System;
using Xunit;

namespace CSharpInternals.ExceptionHandling
{
    public class Basics
    {   
        // Fact - ToString allocates a lot, especially with AggregateException
        // https://referencesource.microsoft.com/#mscorlib/system/AggregateException.cs,448
        
        [Fact]
        public void ThrowsNull()
        {
            Assert.Throws<NullReferenceException>(() => ThrowsNullReferenceException());
        }
        
        private static void ThrowsNullReferenceException() => throw null;
    }
}