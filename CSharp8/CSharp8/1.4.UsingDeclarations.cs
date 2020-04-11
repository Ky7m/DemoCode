using System;
using System.IO;
using Xunit;

namespace CSharp8
{
    public class UsingDeclarations
    {
        [Fact]
        public void UsingBlockModernSyntax()
        {
            using var stream = new MemoryStream();
            stream.Write(ReadOnlySpan<byte>.Empty);
        }

        [Fact]
        public void PatternBasedUsing()
        {
            //using var myDisposable = new MyDisposable(); //error CS1674
            using var myDisposable = new MyDisposableStruct();
        }

        // "pattern-based using" only works with ref struct types,
        // but the error message doesn't mention that.
        // See https://github.com/dotnet/roslyn/issues/33746
        public sealed class MyDisposable //: IDisposable
        {
            public void Dispose()
            {
            }
        }
        
        public ref struct MyDisposableStruct
        {
            public void Dispose()
            {
                Console.WriteLine("Disposed");
            }
        }
    }
}