using System;
using System.Threading;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.ExceptionHandling
{
    public class TryFinallyBlocks : BaseTestHelpersClass
    {
        public TryFinallyBlocks(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void EnvironmentFailFast()
        {
            const string causeOfFailure = "A catastrophic failure has occured.";

            try
            {
                throw new Exception(causeOfFailure);
            }
            catch (Exception exception)
            {
                WriteLine(exception.ToString());
                //Environment.FailFast(causeOfFailure);
            }
            finally
            {
                WriteLine("This finally block will not be executed.");
            }
        }

        [Fact]
        public void ThreadAbortExceptionAndFinally()
        {
#if NET47
            var testThread = new Thread(() =>
            {
                try
                {
                    WriteLine("test thread started at " + DateTime.Now);
                    while (true)
                    {
                        Thread.Sleep(500);
                        WriteLine("test thread worked at " + DateTime.Now);
                    }
                }
                finally // Thread.BeginCriticalRegion(); // .NET 2.0+
                {
                    WriteLine("test thread entered FINALLY at " + DateTime.Now);
                } //  Thread.EndCriticalRegion(); // .NET 2.0+
            });
            testThread.Start();
            Thread.Sleep(5000);
            if (testThread.IsAlive)
            {
                testThread.Abort();
            }
            WriteLine("main thread after abort call " + DateTime.Now);
#endif
        }

        [Fact]
        public void CancellationTokenAndFinally()
        {
            using (var cts = new CancellationTokenSource())
            {
                var testThread = new Thread(obj =>
                {
                    try
                    {
                        WriteLine("test thread started at " + DateTime.Now);
                        if (obj is CancellationToken token)
                        {
                            while (true)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    WriteLine("cancellation has been requested...");
                                    break;
                                }
                                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                                WriteLine("test thread worked at " + DateTime.Now);
                            }
                        }
                    }
                    finally
                    {
                        WriteLine("test thread entered FINALLY at " + DateTime.Now);
                    }
                });

                testThread.Start(cts.Token);
                Thread.Sleep(TimeSpan.FromSeconds(5));

                if (testThread.IsAlive)
                {
                    cts.Cancel();
                    WriteLine("Cancellation set in token source...");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                WriteLine("main thread after abort call " + DateTime.Now);
            }
        }
    }
}
