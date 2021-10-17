using System;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class CompareStringsRunner
    {
        // use no string literals, which would be interned, placed in the executable.
        private readonly string _a1 = $"{1}{1}";
        private readonly string _a2 = $"{1}{2}";

        private readonly string _b1 = $"{1}{1}{1}{1}{1}{1}";
        private readonly string _b2 = $"{1}{1}{1}{2}{2}{2}";

        private readonly string _c1 = $"{1}{1}{1}{1}{1}{1}{1}{1}{1}{1}{1}{1}";
        private readonly string _c2 = $"{1}{1}{1}{1}{1}{1}{2}{2}{2}{2}{2}{2}";

        [Benchmark]
        public bool Common()
        {
            return _a1 == _a2 &&
                _b1 == _b2 &&
                _c1 == _c2 &&
                _a1 == _c1 &&
                _b2 == _c2;
        }

        [Benchmark]
        public bool Equals()
        {
            return _a1.Equals(_a2) &&
                _b1.Equals(_b2) &&
                _c1.Equals(_c2) &&
                _a1.Equals(_c1) &&
                _b2.Equals(_c2);
        }

        [Benchmark]
        public bool StringEquals()
        {
            return string.Equals(_a1, _a2) &&
                string.Equals(_b1, _b2) &&
                string.Equals(_c1, _c2) &&
                string.Equals(_a1, _c1) &&
                string.Equals(_b2, _c2);
        }

        [Benchmark]
        public bool StringCompare()
        {
#pragma warning disable RECS0119 // Warns when a culture-aware 'Compare' call is used by default
            return string.Compare(_a1, _a2) == 0 &&
                string.Compare(_b1, _b2) == 0 &&
                string.Compare(_c1, _c2) == 0 &&
                string.Compare(_a1, _c1) == 0 &&
                string.Compare(_b2, _c2) == 0;
#pragma warning restore RECS0119 // Warns when a culture-aware 'Compare' call is used by default
        }

        [Benchmark]
        public bool StringCompareOrdinal()
        {
            return string.CompareOrdinal(_a1, _a2) == 0 &&
                string.CompareOrdinal(_b1, _b2) == 0 &&
                string.CompareOrdinal(_c1, _c2) == 0 &&
                string.CompareOrdinal(_a1, _c1) == 0 &&
                string.CompareOrdinal(_b2, _c2) == 0;
        }

        [Benchmark]
        public bool StringCompareIgnoreCase()
        {
#pragma warning disable RECS0119
            return string.Compare(_a1, _a2, true) == 0 &&

                string.Compare(_b1, _b2, true) == 0 &&
                string.Compare(_c1, _c2, true) == 0 &&
                string.Compare(_a1, _c1, true) == 0 &&
                string.Compare(_b2, _c2, true) == 0;
#pragma warning restore RECS0119
        }

        [Benchmark]
        public bool EqualsCurrentCulture()
        {
            return _a1.Equals(_a2, StringComparison.CurrentCulture) &&
                _b1.Equals(_b2, StringComparison.CurrentCulture) &&
                _c1.Equals(_c2, StringComparison.CurrentCulture) &&
                _a1.Equals(_c1, StringComparison.CurrentCulture) &&
                _b2.Equals(_c2, StringComparison.CurrentCulture);
        }

        [Benchmark]
        public bool StringEqualsCurrentCulture()
        {
            return string.Equals(_a1, _a2, StringComparison.CurrentCulture) &&
                string.Equals(_b1, _b2, StringComparison.CurrentCulture) &&
                string.Equals(_c1, _c2, StringComparison.CurrentCulture) &&
                string.Equals(_a1, _c1, StringComparison.CurrentCulture) &&
                string.Equals(_b2, _c2, StringComparison.CurrentCulture);
        }

        [Benchmark]
        public bool EqualsCurrentCultureIgnoreCase()
        {
            return _a1.Equals(_a2, StringComparison.CurrentCultureIgnoreCase) &&
                _b1.Equals(_b2, StringComparison.CurrentCultureIgnoreCase) &&
                _c1.Equals(_c2, StringComparison.CurrentCultureIgnoreCase) &&
                _a1.Equals(_c1, StringComparison.CurrentCultureIgnoreCase) &&
                _b2.Equals(_c2, StringComparison.CurrentCultureIgnoreCase);
        }

        [Benchmark]
        public bool StringEqualsCurrentCultureIgnoreCase()
        {
            return string.Equals(_a1, _a2, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(_b1, _b2, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(_c1, _c2, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(_a1, _c1, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(_b2, _c2, StringComparison.CurrentCultureIgnoreCase);
        }

        [Benchmark]
        public bool EqualsOrdinalCulture()
        {
            return _a1.Equals(_a2, StringComparison.Ordinal) &&
                _b1.Equals(_b2, StringComparison.Ordinal) &&
                _c1.Equals(_c2, StringComparison.Ordinal) &&
                _a1.Equals(_c1, StringComparison.Ordinal) &&
                _b2.Equals(_c2, StringComparison.Ordinal);
        }

        [Benchmark]
        public bool StringEqualsOrdinalCulture()
        {
            return string.Equals(_a1, _a2, StringComparison.Ordinal) &&
                string.Equals(_b1, _b2, StringComparison.Ordinal) &&
                string.Equals(_c1, _c2, StringComparison.Ordinal) &&
                string.Equals(_a1, _c1, StringComparison.Ordinal) &&
                string.Equals(_b2, _c2, StringComparison.Ordinal);
        }

        [Benchmark]
        public bool EqualsOrdinalCultureIgnoreCase()
        {
            return _a1.Equals(_a2, StringComparison.OrdinalIgnoreCase) &&
                _b1.Equals(_b2, StringComparison.OrdinalIgnoreCase) &&
                _c1.Equals(_c2, StringComparison.OrdinalIgnoreCase) &&
                _a1.Equals(_c1, StringComparison.OrdinalIgnoreCase) &&
                _b2.Equals(_c2, StringComparison.OrdinalIgnoreCase);
        }

        [Benchmark]
        public bool StringEqualsOrdinalCultureIgnoreCase()
        {
            return string.Equals(_a1, _a2, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_b1, _b2, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_c1, _c2, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_a1, _c1, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(_b2, _c2, StringComparison.OrdinalIgnoreCase);
        }

        [Benchmark]
        public bool CompareTo()
        {
#pragma warning disable RECS0064 // Warns when a culture-aware 'string.CompareTo' call is used by default
            return _a1.CompareTo(_a2) == 0 &&
                _b1.CompareTo(_b2) == 0 &&
                _c1.CompareTo(_c2) == 0 &&
                _a1.CompareTo(_c1) == 0 &&
                _b2.CompareTo(_c2) == 0;
#pragma warning restore RECS0064 // Warns when a culture-aware 'string.CompareTo' call is used by default
        }
    }
}