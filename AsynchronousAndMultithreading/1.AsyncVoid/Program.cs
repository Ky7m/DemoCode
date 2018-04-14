using System;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncVoid
{
    class Program
    {
        static async Task Main()
        {
            WriteLine("Started");
            try
            {
                VoidAsyncAndThrow();
            }
            catch (Exception)
            {
                WriteLine("Catch");
            }

            await Task.Delay(2000);
            WriteLine("Finished");
        }
        
        private static async void VoidAsyncAndThrow()
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }
}