using System;
using Xunit;

namespace CSharp8
{
    public class NestedStackalloc
    {
        [Fact]
        public void NewSyntax()
        {
            // Fine before C# 8
            Span<int> span = stackalloc int[5];
            M(span);

            // Only valid in C# 8
            M(stackalloc int[10]);
        }
        
        void M(Span<int> span){}
    }
}