using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace AsyncPerformance
{
    class Program
    {
        static void Main(string[] args) => 
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
            .Run(args, new BenchmarkConfig());
    }
    
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MemoryDiagnoser.Default);

            Add(DefaultConfig.Instance.GetValidators().ToArray());
            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());

            Set(new BenchmarkDotNet.Reports.SummaryStyle
            {
                PrintUnitsInHeader = true,
                PrintUnitsInContent = true,
            });
        }
    }
}