using System;
using System.Reflection;

namespace ExceptionDispatchInfoDemo
{
    static class ExceptionExtensions
    {
        public static void PreserveStackTrace(this Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod(
                "InternalPreserveStackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic);

            preserveStackTrace.Invoke(exception, null);
        }
    }
}