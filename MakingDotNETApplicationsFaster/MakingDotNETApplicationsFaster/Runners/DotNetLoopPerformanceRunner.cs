using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class DotNetLoopPerformanceRunner
    {
        private const int ArrayLength = 10000;

        private readonly int[] _array;
        private readonly List<int> _list;

        private int _arrayIndex;

        public DotNetLoopPerformanceRunner()
        {
            _array = Enumerable.Range(0, ArrayLength).ToArray();
            //array[ArrayLength] = 1; // throws error on runtime: that means that the CLR has to inject bounds checking into array access
            _list = _array.ToList();
        }

        [Benchmark]
        public long BaselineLoop()
        {
            long sum = 0;
            for (var i = 0; i < _array.Length; i++)
            {
                sum += _array[i];
            }
            return sum;

        }

        [Benchmark]
        public long BaselineLoopIndexPrefix()
        {
            long sum = 0;
            for (var i = 0; i < _array.Length; ++i)
            {
                sum += _array[i];
            }
            return sum;

        }

        [Benchmark]
        public long GetSumWhile()
        {
            long sum = 0;
            var i = _array.Length;
            while (i-- > 0)
            {
                sum += _array[i];
            }
            return sum;

        }

        [Benchmark]
        public unsafe long UnsafeArrayLinearAccessWithPointerIncrement()
        {
            long sum = 0;
            fixed (int* pointer = &_array[0])
            {
                var current = pointer;

                for (var i = 0; i < _array.Length; ++i)
                {
                    sum += *(current++);
                }
            }

            return sum;
        }

        [Benchmark]
        public unsafe long UnsafeArrayLinearAccess()
        {
            long sum = 0;
            fixed (int* pointer = &_array[0])
            {
                var current = pointer;

                for (var i = 0; i < _array.Length; ++i)
                {
                    sum += *(current + i);
                }
            }

            return sum;
        }

        [Benchmark]
        public long GetSumForeach()
        {
            long sum = 0;
            foreach (var val in _array)
            {
                sum += val;
            }
            return sum;
        }

        [Benchmark]
        public long GetSumLinq()
        {
            return _array.Sum();
        }

        [Benchmark]
        public long GetSumOfListFor()
        {
            long sum = 0;
            for (var i = 0; i < _list.Count; i++)
            {
                sum += _list[i];
            }

            return sum;
        }

        [Benchmark]
        public long GetSumOfListForeach()
        {
            long sum = 0;
            foreach (var val in _list)
            {
                sum += val;
            }

            return sum;
        }

        [Benchmark]
        public long GetSumOfListLinq()
        {
            return _list.Sum();
        }

        [Benchmark]
        public long GetSumOfIEnumerableForeach()
        {
            var collection = _array as IEnumerable<int>;
            long sum = 0;
            foreach (var val in collection)
            {
                sum += val;
            }

            return sum;
        }

        [Benchmark]
        public long GetSumLoopUnrollingArray()
        {
            long sum = 0;
            for (var i = 0; i < _array.Length - 4; i += 4)
            {
                sum += _array[i];
                sum += _array[i + 1];
                sum += _array[i + 2];
                sum += _array[i + 3];
            }

            return sum;
        }

        [Benchmark]
        public long GetSumLoopUnrollingList()
        {
            long sum = 0;
            for (var i = 0; i < _list.Count - 4; i += 4)
            {
                sum += _list[i];
                sum += _list[i + 1];
                sum += _list[i + 2];
                sum += _list[i + 3];
            }

            return sum;
        }

        [Benchmark]
        public long GetSumWithPrecalculatedLength()
        {
            long sum = 0;
            for (var i = 0; i < ArrayLength; i++)
            {
                sum += _array[i];
            }

            return sum;
        }

        [Benchmark]
        public long GetSumWithGoToOperator()
        {
            long sum = 0;
            var i = 0;
            next:
            if (i < ArrayLength)
            {
                sum+=_array[i];
                i++;
                goto next;
            }

            return sum;
        }
        
        [Benchmark]
        public long GetSumOfRecursion()
        {
            _arrayIndex = ArrayLength;
            return GetSumRecursively();
        }

        private long GetSumRecursively()
        {
            if (_arrayIndex <= 0)
            {
                return 0;
            }

            return _array[--_arrayIndex] + GetSumRecursively();
        }

        [Benchmark]
        public long GetSumOfTailRecursion()
        {
            _arrayIndex = ArrayLength - 1;
            return GetSumOfTailRecursionInternal(0, _arrayIndex);
        }

        private long GetSumOfTailRecursionInternal(long sum, int index)
        {
            if (index < 0)
            {
                return sum;
            }

            return GetSumOfTailRecursionInternal(sum + _array[index], index - 1);
        }
    }
}
