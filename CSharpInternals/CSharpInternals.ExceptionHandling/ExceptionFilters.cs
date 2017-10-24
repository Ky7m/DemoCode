using System;
using System.Collections;
using System.Threading.Tasks;
using CSharpInternals.Utils;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.ExceptionHandling
{
    [UsedImplicitly]
    public class ExceptionFilters : BaseTestHelpersClass
    {
        public ExceptionFilters(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ExceptionFilter()
        {
            var exception = await Assert.ThrowsAsync<Exception>(DoSomethingWithExceptionFilter);
            WriteLine(exception.ToString());
        }
        
        [Fact]
        public async Task CatchBlockFilter()
        {
            var exception = await Assert.ThrowsAsync<Exception>(DoSomethingWithCatchBlockFilter);
            WriteLine(exception.ToString());
        }

        private Task DoSomethingWithExceptionFilter()
        {
            try
            {
                return DoSomethingWork();
            }
            catch (Exception e) when(!IsExceptionCritical(e))
            {
                WriteLine(e);
            }
            
            return Task.CompletedTask;
        }
        
        private Task DoSomethingWithCatchBlockFilter()
        {
            try
            {
                return DoSomethingWork();
            }
            catch (Exception e)
            {
                if (!IsExceptionCritical(e))
                {
                    WriteLine(e);
                }
                else
                {
                    throw;
                }
            }
            
            return Task.CompletedTask;
        }
        
        private static Task DoSomethingWork()
        {
            throw new Exception
            {
                Data =
                {
                    {"ErrorCode", -1}
                }
            };
        }
        
        private static bool IsExceptionCritical(Exception exception)
        {
            foreach (DictionaryEntry item in exception.Data)
            {
                var hasErrorCode = string.Equals("ErrorCode", item.Key.ToString(), StringComparison.OrdinalIgnoreCase);
                if (hasErrorCode)
                {
                    if (item.Value is int itemValue && itemValue == -1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}