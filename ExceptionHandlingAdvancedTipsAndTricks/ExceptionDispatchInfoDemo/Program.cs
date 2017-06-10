using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace ExceptionDispatchInfoDemo
{
    static class ExceptionDispatchInfoDemoProgram
    {

        public static void CommonWay()
        {
            Exception myEx;
            try
            {
                throw new Exception("I failed...");
            }
            catch (Exception ex)
            {
                myEx = ex;
            }

            if (myEx != null)
            {
                throw myEx;
            }
        }

        public static void BestWay()
        {
            ExceptionDispatchInfo myEx;
            try
            {
                throw new Exception("I failed...");
            }
            catch (Exception ex)
            {
                myEx = ExceptionDispatchInfo.Capture(ex);
            }

            myEx.Throw();
        }


        public static void BestWayCompatibility()
        {
            Exception myEx;

            try
            {
                throw new Exception("I failed...");
            }
            catch (Exception ex)
            {
                ex.PreserveStackTrace();
                myEx = ex;
            }


            if (myEx != null)
            {
                throw myEx;
            }
        }


        static void Main()
        {
            var exceptions = new BlockingCollection<ExceptionDispatchInfo>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    ThrowOne();
                }
                catch (Exception ex)
                {
                    var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);

                    exceptions.Add(exceptionDispatchInfo);
                }
                exceptions.CompleteAdding();
            });

            foreach (var exceptionDispatchInfo in exceptions.GetConsumingEnumerable())
            {
                try
                {
                    exceptionDispatchInfo.Throw();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
            }

            Console.ReadKey();
        }

        private static void ThrowOne()
        {
            Console.WriteLine("Throw Not Supported Exception");

            ThrowTwo();

            throw new NotSupportedException();
        }

        private static void ThrowTwo()
        {
            Console.WriteLine("Throw Not Implemented Exception");

            ThrowThree();

            throw new NotImplementedException();
        }

        private static void ThrowThree()
        {
            Console.WriteLine("Throw Argument Null Exception");

            throw new ArgumentNullException();
        }
    }
}
