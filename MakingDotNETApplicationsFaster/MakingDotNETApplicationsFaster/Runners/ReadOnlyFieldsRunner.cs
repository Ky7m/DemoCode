using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class ReadOnlyFieldsRunner
    {
        private Int256 _value = new Int256(1L, 5L, 10L, 100L);
        private readonly Int256 _readOnlyValue = new Int256(1L, 5L, 10L, 100L);

        [Benchmark]
        private long GetValue()
        {
            return _value.Bits0 + _value.Bits1 + _value.Bits2 + _value.Bits3;
        }

        [Benchmark]
        private long GetReadOnlyValue()
        {
            return _readOnlyValue.Bits0 + _readOnlyValue.Bits1 + _readOnlyValue.Bits2 + _readOnlyValue.Bits3;
        }
    }

    public struct Int256
    {
        public Int256(long bits0, long bits1, long bits2, long bits3)
        {
            Bits0 = bits0;
            Bits1 = bits1;
            Bits2 = bits2;
            Bits3 = bits3;
        }
        public long Bits0 { get; set; }
        public long Bits1 { get; set; }
        public long Bits2 { get; set; }
        public long Bits3 { get; set; }
    }
}