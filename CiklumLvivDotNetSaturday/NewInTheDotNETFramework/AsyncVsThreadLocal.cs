using System;
using System.Threading;
using System.Threading.Tasks;

namespace NewInTheDotNETFramework
{
    internal static class AsyncVsThreadLocal
    {
        private static AsyncLocal<string> s_asyncLocalString = new AsyncLocal<string>();

        private static ThreadLocal<string> s_threadLocalString = new ThreadLocal<string>();

        public static async Task AsyncMethodA()
        {
            s_asyncLocalString.Value = "Value 1";
            s_threadLocalString.Value = "Value 1";
            var t1 = AsyncMethodB("Value 1");

            s_asyncLocalString.Value = "Value 2";
            s_threadLocalString.Value = "Value 2";
            var t2 = AsyncMethodB("Value 2");

            await t1;
            await t2;
        }

        private static async Task AsyncMethodB(string expectedValue)
        {
            Console.WriteLine("Entering AsyncMethodB.");
            Console.WriteLine("   Expected '{0}', AsyncLocal value is '{1}', ThreadLocal value is '{2}'",
                              expectedValue, s_asyncLocalString.Value, s_threadLocalString.Value);
            await Task.Delay(100);
            Console.WriteLine("Exiting AsyncMethodB.");
            Console.WriteLine("   Expected '{0}', got '{1}', ThreadLocal value is '{2}'",
                              expectedValue, s_asyncLocalString.Value, s_threadLocalString.Value);
        }
    }
}
