using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using CSharpInternals.Utils;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.ExceptionHandling
{
    [UsedImplicitly]
    public class ExceptionDispatch : BaseTestHelpersClass
    {
        public ExceptionDispatch(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task RethrowWithHack()
        {
            Exception myEx;

            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                ex.PreserveStackTrace();
                myEx = ex;
            }
            var exception = await Assert.ThrowsAsync<Exception>(() => throw myEx);
            WriteLine(exception.ToString());
        }

        [Fact]
        public async Task RethrowWithExceptionDispatchInfo()
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

            var exception = await Assert.ThrowsAsync<Exception>(() =>
            {
                myEx.Throw();
                return Task.CompletedTask;
            });
            WriteLine(exception.ToString());
        }
    }

    static class ExceptionExtensions
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
