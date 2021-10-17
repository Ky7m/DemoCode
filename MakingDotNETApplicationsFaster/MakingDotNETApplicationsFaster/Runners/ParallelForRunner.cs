using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]

    public class ParallelForRunner
    {
        private const int Iterations = 10000;
        private readonly OrderablePartitioner<Tuple<int, int>> _partitioner;
        private readonly int[] _array;

        public ParallelForRunner()
        {
            _partitioner = Partitioner.Create(0, Iterations);
            _array = Enumerable.Range(0, Iterations).ToArray();
        }

        [Benchmark]
        public void ParallelFor()
        {
            long sum = 0;
            Parallel.For(0, Iterations, i =>
            {
                Interlocked.Add(ref sum, (long)Math.Sqrt(i));
            });
        }

        [Benchmark]
        public void ParallelForEach()
        {
            long sum = 0;

            Parallel.ForEach(_array, i =>
            {
                Interlocked.Add(ref sum, (long)Math.Sqrt(i));
            });
        }

        [Benchmark]
        public void ParallelForEachPartioner()
        {
            long sum = 0;

            Parallel.ForEach(_partitioner,
                (range) =>
                {
                    long partialSum = 0;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        partialSum += (long)Math.Sqrt(i);
                    }

                    Interlocked.Add(ref sum, partialSum);
                });
        }
    }
}
