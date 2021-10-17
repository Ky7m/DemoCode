using BenchmarkDotNet.Running;
using MakingDotNETApplicationsFaster.Runners;

namespace MakingDotNETApplicationsFaster
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<ParallelForRunner>();
        }
    }
}
