using BenchmarkDotNet.Attributes;

namespace CSharpInternals
{
    public class UndocumentedKeywordsBenchmarks
    {
        private const int IterationCount = 10000000;
        private readonly int[] _array;
        
        public UndocumentedKeywordsBenchmarks()
        {
            _array = new int[5];
        }
        
        [Benchmark]
        public void MakeRef()
        {
            for (var i = 0; i < IterationCount; i++)
            {
                Set1(_array, 0, i);
            }
        }

        [Benchmark]
        public void Boxing()
        {
            for (var i = 0; i < IterationCount; i++)
            {
                Set2(_array, 0, i);
            }
        }

        private static void Set1<T>(T[] a, int i, int v)
        {
            __refvalue(__makeref(a[i]), int) = v;
        }

        private static void Set2<T>(T[] a, int i, int v)
        {
            a[i] = (T)(object)v;
        }   
    }
}