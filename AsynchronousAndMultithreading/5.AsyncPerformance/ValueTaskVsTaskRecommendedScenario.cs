using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AsyncPerformance
{
    public class ValueTaskVsTaskRecommendedScenario
    {
        private const int Count = 100000;
        private int _index;
        private readonly int[] _result;

        public ValueTaskVsTaskRecommendedScenario()
        {
            _index = 0;
            _result = new int[Count];
        }

        [Benchmark]
        public Task<int[]> Reference()
        {
            return ReadAsync();
        }
        
        [Benchmark]
        public Task<int[]> ValueType()
        {
            return ReadMixedAsync();
        }

        private async Task<int[]> ReadAsync()
        {
            _index = 0;
            while (_index < Count)
            {
                _result[_index] += await AsynchronousOperation();
                _index++;
            }
            return _result;
        }

        private Task<int> AsynchronousOperation()
        {
            if (IsSynchronousOperationPossible())
            {
                return Task.FromResult(SynchronousOperation());
            }

            return Task.Factory.StartNew(SynchronousOperation);
        }

        private bool IsSynchronousOperationPossible() => _index % 20 != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SynchronousOperation() => _index * _index * 20;

        private async Task<int[]> ReadMixedAsync()
        {
            _index = 0;
            while (_index < Count)
            {
                var valueTask = MixedOperation();
                if (valueTask.IsCompletedSuccessfully)
                {
                    _result[_index] = valueTask.Result;
                }
                else
                {
                    _result[_index] = await valueTask.AsTask();
                }
                _index++;
            }

            return _result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ValueTask<int> MixedOperation()
        {
            if (IsSynchronousOperationPossible())
            {
                return new ValueTask<int>(SynchronousOperation());
            }

            return new ValueTask<int>(Task.Factory.StartNew(SynchronousOperation));
        }
    }
}