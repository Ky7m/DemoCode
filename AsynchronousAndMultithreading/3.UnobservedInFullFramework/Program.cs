using System;
using System.Threading.Tasks;
using static System.Console;

namespace UnobservedInFullFramework
{
    internal class Program
    {
        public static async Task Main()
        {
            WriteLine("Started");
            try
            {
                AsyncAndThrow();
            }
            catch (Exception)
            {
                WriteLine("Catch");
            }

            await Task.Delay(2000);
            WriteLine("Finished");
            
            // https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/runtime/throwunobservedtaskexceptions-element
            // GC.Collect(); GC.WaitForPendingFinalizers();
        }
        
        private static async Task AsyncAndThrow()
        {
            await Task.Delay(500);
            throw new InvalidOperationException();
        }
    }
}