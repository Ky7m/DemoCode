using System;
using System.Collections;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.ExceptionHandling
{
    public class ExceptionFilters : BaseTestHelpersClass
    {
        public ExceptionFilters(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void CatchBlockFilter()
        {
            var exception =  Assert.Throws<Exception>(() => DoSomethingWithCatchBlockFilter());
            WriteLine(exception.ToString());
        }
        
        [Fact]
        public void ExceptionFilter()
        {
            var exception = Assert.Throws<Exception>(() => DoSomethingWithExceptionFilter());
            WriteLine(exception.ToString());
        }

        private static void DoSomethingWork()
        {
            throw new Exception
            {
                Data =
                {
                    {"ErrorCode", -1}
                }
            };
        }
        
        private void DoSomethingWithCatchBlockFilter()
        {
            try
            {
                DoSomethingWork();
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
        }
        
        private void DoSomethingWithExceptionFilter()
        {
            try
            {
                DoSomethingWork();
            }
            catch (Exception e) when(!IsExceptionCritical(e))
            {
                WriteLine(e);
            }
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