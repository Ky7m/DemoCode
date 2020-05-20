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
            AddDiagnoser(MemoryDiagnoser.Default);
            AddValidator(DefaultConfig.Instance.GetValidators().ToArray());
            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());

            AddExporter(HtmlExporter.Default);
        }
    }
}