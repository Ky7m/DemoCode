using System;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncGotchas
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WriteLine($"{nameof(DoWork)}.Started");
            await DoWork();
            WriteLine($"{nameof(DoWork)}.Finished");

            WriteLine();
            
            WriteLine($"{nameof(DoWork)}.Started");
            await DoWorkWithAwait();
            WriteLine($"{nameof(DoWork)}.Finished");
        }

        private static Task DoWork()
        {
            using (var service = new DisposableService())
            {
                var a = ComputeArg();
                var b = ComputeArg();
                return service.CalculationAsync(a, b);
            }
        }

        private static async Task DoWorkWithAwait()
        {
            using (var service = new DisposableService())
            {
                var a = ComputeArg();
                var b = ComputeArg();
                await service.CalculationAsync(a, b);
            }
        }

        private static int ComputeArg() => new Random().Next(500,1000);
    }
}