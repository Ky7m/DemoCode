using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace MakingDotNETApplicationsFaster
{
    public class CoreConfig : ManualConfig
    {
        public CoreConfig()
        {
            Orderer = new FastestToSlowestOrderer();
        }
        
        private class FastestToSlowestOrderer : IOrderer
        {
            public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule> order = null) =>
                from benchmark in benchmarksCase
                orderby benchmark.Parameters["X"] descending,
                    benchmark.Descriptor.WorkloadMethodDisplayInfo
                select benchmark;

            IEnumerable<BenchmarkCase> IOrderer.GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary)
            {
                return GetSummaryOrder(benchmarksCases, summary);
            }

            public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) =>
                from benchmark in benchmarksCase
                orderby summary[benchmark].ResultStatistics.Mean
                select benchmark;

            public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => null;
            string IOrderer.GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase)
            {
                return GetLogicalGroupKey(allBenchmarksCases, benchmarkCase);
            }

            public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule> order = null) => 
                logicalGroups.OrderBy(it => it.Key);

            public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
                benchmarkCase.Job.DisplayInfo + "_" + benchmarkCase.Parameters.DisplayInfo;
            
            public bool SeparateLogicalGroups => true;
        }
    }
}