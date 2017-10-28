using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.ExceptionHandling
{
    public class ExceptionDispatch : BaseTestHelpersClass
    {
        public ExceptionDispatch(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void RethrowWithHack()
        {
            Exception myEx;

            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                //ex.PreserveStackTrace();
                myEx = ex;
            }
            
            void ThrowFunction() => throw myEx;
            
            var exception = Assert.Throws<Exception>(() =>
            {
                ThrowFunction();
            });
            
            WriteLine(exception.ToString());
        }

        [Fact]
        public void RethrowWithExceptionDispatchInfo()
        {
            ExceptionDispatchInfo myEx;
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                myEx = ExceptionDispatchInfo.Capture(ex);
            }

            var exception =  Assert.Throws<Exception>(() =>
            {
                myEx.Throw();
            });
            WriteLine(exception.ToString());
        }
    }

    internal static class ExceptionExtensions
    {
        public static void PreserveStackTrace(this Exception exception)
        {
            var preserveStackTrace = typeof(Exception).GetMethod(
                "InternalPreserveStackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic);

            preserveStackTrace.Invoke(exception, null);
        }
    }
}
