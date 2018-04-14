using System;
using System.Threading.Tasks;

namespace AsyncGotchas
{
    internal class DisposableService : IDisposable
    {
        public DisposableService()
        {
            Console.WriteLine($"{nameof(DisposableService)}.ctor");
        }
        
        public async Task CalculationAsync(int a, int b)
        {
            await Task.Delay(a + b);
            Console.WriteLine($"{nameof(DisposableService)}.{nameof(CalculationAsync)}");
        }

        private void Dispose(bool disposing)
        {
            var caller = disposing ? "dispose method" : "finalizer";
            Console.WriteLine($"{nameof(DisposableService)}.Dispose called from {caller}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableService()
        {
            Dispose(false);
        }
    }
}