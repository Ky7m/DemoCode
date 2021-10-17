using System.Text;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class StringConcatVsStringBuilderRunner
    {
        [Params(1, 2, 3, 4, 5, 7, 10, 12, 15, 17, 20)]
        public int Loops;

        [Benchmark]
        public string StringConcat()
        {
            string result = string.Empty;
            for (int i = 0; i < Loops; ++i)
                result = string.Concat(result, i.ToString());
            return result;
        }

        [Benchmark]
        public string SimpleStringConcatenation()
        {
            string result = string.Empty;
            for (int i = 0; i < Loops; ++i)
                result += i.ToString();
            return result;
        }

        [Benchmark]
        public string StringBuilder()
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            for (int i = 0; i < Loops; ++i)
                sb.Append(i.ToString());
            return sb.ToString();
        }
    }
}
