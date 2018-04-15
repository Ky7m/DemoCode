using System;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncLambda
{
    class Program
    {
        static async Task Main()
        {
            WriteLine("Started");
            try
            {
                Process(async () =>
                {
                    await Task.Delay(500);
                    throw new Exception();
                });
            }
            catch (Exception)
            {
                WriteLine("Catch");
            }

            await Task.Delay(2000);
            WriteLine("Finished");
        }
        
        private static void Process(Action action)
        {
            action();
        }
        
//        private static void Process(Func<Task> action)
//        {
//            action();
//        }
    }
}