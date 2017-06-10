using System;
using System.Collections;

namespace WrappingExceptionsInExceptionsDemo
{
    static class WrappingExceptionsInExceptionsDemoProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nException with some extra information...");
            RunTest(false);
            Console.WriteLine("\nException with all extra information...");
            RunTest(true);
        }

        private static void RunTest(bool displayDetails)
        {
            try
            {
                NestedRoutine1(displayDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception was thrown.");
                Console.WriteLine(e.Message);
                if (e.Data.Count > 0)
                {
                    Console.WriteLine("  Extra details:");
                    foreach (DictionaryEntry de in e.Data)
                    {
                        Console.WriteLine("    Key: {0,-20}      Value: {1}", "'" + de.Key.ToString() + "'", de.Value);
                    }
                }
            }
        }

        private static void NestedRoutine1(bool displayDetails)
        {
            try
            {
                NestedRoutine2(displayDetails);
            }
            catch (Exception e)
            {
                e.Data["ExtraInfo"] = "Information from NestedRoutine1.";
                e.Data.Add("MoreExtraInfo", "More information from NestedRoutine1.");
                throw;
            }
        }

        private static void NestedRoutine2(bool displayDetails)
        {
            var e = new Exception("This statement is the original exception message.");
            if (displayDetails)
            {
                var s = "Information from NestedRoutine2.";
                var i = -903;
                var dt = DateTime.Now;
                e.Data.Add("stringInfo", s);
                e.Data["IntInfo"] = i;
                e.Data["DateTimeInfo"] = dt;
            }
            throw e;
        }
    }
}
