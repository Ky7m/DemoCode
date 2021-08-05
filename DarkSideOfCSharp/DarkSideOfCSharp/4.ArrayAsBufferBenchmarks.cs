using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace DarkSideOfCSharp
{
    public class ArrayAsBufferBenchmarks
    {
        private const int ArraySize = 1_000_000;
        
        [Benchmark]
         public void RegularArray()
         {
             var array = new byte[ArraySize];
         }
        
         [Benchmark]
         public void SharedArrayPool()
         {
             var pool = ArrayPool<byte>.Shared;
             var array = pool.Rent(ArraySize);
             try
             {
                 // Process
             }
             finally // Ensure this runs even if the main code throws.
             {
                 pool.Return(array);
             }
         }
    }
}