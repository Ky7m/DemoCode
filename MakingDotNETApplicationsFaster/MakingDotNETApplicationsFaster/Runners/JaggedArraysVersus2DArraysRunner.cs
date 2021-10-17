using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class JaggedArraysVersus2DArraysRunner
    {
        private const int Increment = 1309;
        private const int Rows = 1000;
        private const int Cols = 100;
        private readonly int[][] _jaggedArray;
        private readonly int[,] _array;

        public JaggedArraysVersus2DArraysRunner()
        {
            _jaggedArray = new int[Rows][];
            for (var i = 0; i < Rows; i++)
            {
                _jaggedArray[i] = new int[Cols];
            }
            _jaggedArray[0][0] = 0;

            _array = new int[Rows, Cols];

            _array[0, 0] = 0;
        }
        [Benchmark]
        public long GetSumFor2DArray()
        {
            long sum = 0;
            for (var i = 0; i < Rows; ++i)
            {
                for (var j = 0; j < Cols; ++j)
                {
                    sum += _array[i, j];
                }
            }
            return sum;

        }

        [Benchmark]
        public long GetSumForJaggedArray()
        {
            long sum = 0;
            for (var i = 0; i < Rows; ++i)
            {
                for (var j = 0; j < Cols; ++j)
                {
                    sum += _jaggedArray[i][j];
                }
            }
            return sum;

        }

        [Benchmark]
        public long GetSumForJaggedArrayWithCachingTo1DArray()
        {
            long sum = 0;
            for (var i = 0; i < Rows; ++i)
            {
                var theRow = _jaggedArray[i];
                for (var j = 0; j < Cols; ++j)
                {
                    sum += theRow[j];
                }
            }
            return sum;

        }

        [Benchmark]
        public void Traversal2DArray()
        {
            for (var i = 0; i < Rows; ++i)
            {
                for (var j = 0; j < Cols; ++j)
                {
                    _array[i, j] = int.MaxValue - _array[i, j];
                }
            }
        }

        [Benchmark]
        public void TraversalJaggedArray()
        {
            for (var i = 0; i < Rows; ++i)
            {
                for (var j = 0; j < Cols; ++j)
                {
                    _jaggedArray[i][j] = int.MaxValue - _jaggedArray[i][j];
                }
            }
        }

        [Benchmark]
        public void OptimizedTraversal2DArray()
        {
            var count = (((long)Rows) * Cols) / 3;
            unsafe
            {
                fixed (int* pArray = _array)
                {
                    var p = pArray;
                    while (count-- > 0)
                    {
                        *p++ = int.MaxValue - *p;
                        *p++ = int.MaxValue - *p;
                        *p++ = int.MaxValue - *p;
                    }
                }
            }
        }

        [Benchmark]
        public void OptimizedTraversalJagged()
        {
            for (var i = 0; i < Rows; ++i)
            {
                unsafe
                {
                    var count = Cols / 3;
                    fixed (int* pArray = _jaggedArray[i])
                    {
                        var p = pArray;
                        while (count-- > 0)
                        {
                            *p++ = int.MaxValue - *p;
                            *p++ = int.MaxValue - *p;
                            *p++ = int.MaxValue - *p;
                        }
                    }
                }
            }
        }

        [Benchmark]
        public void SemiRandomAccess2DArray()
        {
            var count = Rows * Cols;
            var row = 0;
            var col = 0;
            while (count-- > 0)
            {
                row = (row + Increment) % Rows;
                col = (col + Increment) % Cols;
                _array[row, col] = int.MaxValue - _array[row, col];
            }
        }

        [Benchmark]
        public void SemiRandomAccessJaggedArray()
        {
            var count = Rows * Cols;
            var row = 0;
            var col = 0;
            while (count-- > 0)
            {
                row = (row + Increment) % Rows;
                col = (col + Increment) % Cols;
                _jaggedArray[row][col] = int.MaxValue - _jaggedArray[row][col];
            }
        }
    }
}
