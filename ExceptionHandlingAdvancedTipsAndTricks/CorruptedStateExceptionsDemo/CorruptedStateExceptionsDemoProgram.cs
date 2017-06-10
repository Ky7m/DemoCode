using System;

/*
EXCEPTION_ILLEGAL_INSTRUCTION EXCEPTION_IN_PAGE_ERROR
EXCEPTION_INVALID_DISPOSITION EXCEPTION_NONCONTINUABLE_EXCEPTION
EXCEPTION_ACCESS_VIOLATION EXCEPTION_STACK_OVERFLOW - IF CLR 2.0
EXCEPTION_PRIV_INSTRUCTION STATUS_UNWIND_CONSOLIDATE
*/

namespace CorruptedStateExceptionsDemo
{
    static class CorruptedStateExceptionsDemoProgram
    {
        //[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        //[System.Security.SecurityCritical]
        static void Main(string[] args)
        {
            try
            {
                Main(args);
               // AccessViolation();
            }
            catch (Exception)
            {
                Console.WriteLine("catch (Exception)");
            }

            Console.WriteLine("Done");
        }

        private static unsafe void AccessViolation()
        {
            byte b = *(byte*)(8762765876);
        }
    }
}
