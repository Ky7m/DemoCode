using System;
using System.Threading;

namespace TryFinallyBlocksDemo
{
    static class TryFinallyBlocksDemoProgram
    {
        static void Main(string[] args)
        {
            EnvironmentFailFast();
            //ThreadAbortExceptionAndFinally();
        }

       
        private static void EnvironmentFailFast()
        {
            var causeOfFailure = "A catastrophic failure has occured.";

            try
            {
                throw new Exception(causeOfFailure);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Environment.FailFast(causeOfFailure);
            }
            finally
            {
                Console.WriteLine("This finally block will not be executed.");
            }
        }

        private static void ThreadAbortExceptionAndFinally()
        {
            var testThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("test thread started at " + DateTime.Now);
                    while (true)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine("test thread worked at " + DateTime.Now);
                    }
                }
                finally
                {
                    Console.WriteLine("test thread entered FINALLY at " + DateTime.Now);
                }
            });
            testThread.Start();
            Thread.Sleep(5000);
            if (testThread.IsAlive)
            {
                testThread.Abort();
            }
            Console.WriteLine("main thread after abort call " + DateTime.Now);
        }
    }
}
