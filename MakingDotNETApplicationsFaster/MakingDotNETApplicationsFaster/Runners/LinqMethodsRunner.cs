using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class LinqMethodsRunner
    {
        private readonly int[] _array;
        private readonly List<int> _list;
        private readonly IEnumerable<int> _range;

        private const int Value = 100;

        public LinqMethodsRunner()
        {
            _range = Enumerable.Range(0, 100000);
            _array = _range.ToArray();
            _list = _range.ToList();
        }

        [Benchmark]
        public int FindByForeachForList()
        {
            foreach (var val in _list)
            {
                if (val == Value)
                {
                    return val;
                }
            }

            return 0;
        }

        [Benchmark]
        public int FindForList()
        {
            return _list.Find(x => x == Value);
        }

        [Benchmark]
        public int FirstOrDefaultForList()
        {
            return _list.FirstOrDefault(x => x == Value);
        }

        [Benchmark]
        public int FirstOrDefaultForArray()
        {
            return _array.FirstOrDefault(x => x == Value);
        }

        [Benchmark]
        public int SingleOrDefaultForList()
        {
            return _list.SingleOrDefault(x => x == Value);
        }

        [Benchmark]
        public int SingleOrDefaultForArray()
        {
            return _array.SingleOrDefault(x => x == Value);
        }

        [Benchmark]
        public int[] ListToArray()
        {
            return _list.ToArray();
        }

        [Benchmark]
        public List<int> ArrayToList()
        {
            return _array.ToList();
        }

        [Benchmark]
        public int[] EnumerableToArray()
        {
            return _range.ToArray();
        }

        [Benchmark]
        public List<int> EnumerableToList()
        {
            return _range.ToList();
        }

        [Benchmark]
        public bool Length()
        {
            return _array.Length > 0;
        }

        [Benchmark]
        public bool Any()
        {
            return _array.Any();
        }

        [Benchmark]
        public bool Count()
        {
            return _array.Count() > 0;
        }

        [Benchmark]
        public List<int> WhereForList()
        {
            return _list.Where(x => x > Value).ToList();
        }

        [Benchmark]
        public List<int> FindAllForList()
        {
            return _list.FindAll(x => x > Value);
        }
    }
}