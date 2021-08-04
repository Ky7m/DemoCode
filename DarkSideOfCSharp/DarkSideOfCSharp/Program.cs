using BenchmarkDotNet.Running;

namespace DarkSideOfCSharp
{
    internal class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
