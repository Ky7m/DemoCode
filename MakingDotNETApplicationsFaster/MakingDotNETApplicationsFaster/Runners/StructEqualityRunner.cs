using BenchmarkDotNet.Attributes;
using MakingDotNETApplicationsFaster.Runners.Models;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class StructEqualityRunner
    {
        private StructWithNoRefType _a;
        private StructWithNoRefType _b;

        private StructWithRefType _c;
        private StructWithRefType _d;

        private StructWithRefTypeAndOverridenEquals _x;
        private StructWithRefTypeAndOverridenEquals _y;

        private StructWithRefTypeAndEquatableImplementation _m;
        private StructWithRefTypeAndEquatableImplementation _n;

        public StructEqualityRunner()
        {
            _a = new StructWithNoRefType();
            _b = new StructWithNoRefType();

            _c = new StructWithRefType();
            _d = new StructWithRefType();

            _x = new StructWithRefTypeAndOverridenEquals();
            _y = new StructWithRefTypeAndOverridenEquals();

            _m = new StructWithRefTypeAndEquatableImplementation();
            _n = new StructWithRefTypeAndEquatableImplementation();
        }

        [Benchmark]
        public bool CompareStructsWithNoRefTypes()
        {
            return _a.Equals(_b);
        }

        [Benchmark]
        public bool CompareStructsWithRefTypes()
        {
            return _c.Equals(_d);
        }

        [Benchmark]
        public bool CompareStructsWithRefTypesAndOverridenEquals()
        {
            return _x.Equals(_y);
        }

        [Benchmark]
        public bool CompareStructsWithRefTypesAndEquatableImplementation()
        {
            return _m.Equals(_n);
        }
    }
}