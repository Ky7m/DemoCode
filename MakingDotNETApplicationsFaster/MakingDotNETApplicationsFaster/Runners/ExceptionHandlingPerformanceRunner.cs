using System;
using BenchmarkDotNet.Attributes;

namespace MakingDotNETApplicationsFaster.Runners
{
    [Config(typeof(CoreConfig))]
    public class ExceptionHandlingPerformanceRunner
    {
        private const int Length = 10000;

        [Benchmark]
        public void TryCatchInsideInnerLoop()
        {
            for (var i = 0; i < Length; i++)
            {
                try
                {
                    var value = i * 100;
                    if (value == -1)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        [Benchmark]
        public void TryCatchOutsideInnerLoop()
        {
            try
            {
                for (var i = 0; i < Length; i++)
                {
                    var value = i * 100;
                    if (value == -1)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
