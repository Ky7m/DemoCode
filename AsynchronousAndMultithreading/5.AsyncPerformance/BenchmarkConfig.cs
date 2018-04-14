using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;

namespace AsyncPerformance
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MemoryDiagnoser.Default);

            Add(DefaultConfig.Instance.GetValidators().ToArray());
            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());

            Add(HtmlExporter.Default);
            
            Set(new BenchmarkDotNet.Reports.SummaryStyle
            {
                PrintUnitsInHeader = true,
                PrintUnitsInContent = true,
            });
        }
    }
}