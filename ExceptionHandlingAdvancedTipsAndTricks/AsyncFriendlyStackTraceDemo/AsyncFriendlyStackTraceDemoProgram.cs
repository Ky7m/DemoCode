using System;
using System.Threading.Tasks;
using AsyncFriendlyStackTrace;

namespace AsyncFriendlyStackTraceDemo
{
    static class AsyncFriendlyStackTraceDemoProgram
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine("OLD");
                Console.WriteLine("---");
                Console.WriteLine("```");
                Console.WriteLine(e.ToString());
                Console.WriteLine("```");
                Console.WriteLine();

                Console.WriteLine("NEW");
                Console.WriteLine("---");
                Console.WriteLine("```");
                Console.WriteLine(e.ToAsyncString());
                Console.WriteLine("```");
            }
            
        }

        static async Task MainAsync()
        {
            await new SimpleAsyncMethodChain().Run();
            //await new AsyncInvocationsWithWait().Run();
        }
    }

    public class SimpleAsyncMethodChain
    {
        public async Task Run()
        {
            await A();
        }

        private async Task A()
        {
            await B();
        }

        private async Task B()
        {
            await C();
        }

        private async Task C()
        {
            await Task.Yield();
            throw new Exception("Crash! Boom! Bang!");
        }
    }

    public class AsyncInvocationsWithWait
    {
        public async Task Run()
        {
            await A();
        }

        private async Task A()
        {
            B().Wait();
            await Task.Yield();
        }

        private async Task B()
        {
            await C();
        }

        private async Task C()
        {
            await Task.Yield();
            throw new Exception("Crash! Boom! Bang!");
        }
    }
}
