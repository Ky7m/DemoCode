using System;
using System.Threading.Tasks;

namespace UnderstandingAsync
{
    class Program
    {
        // https://sharplab.io/#v2:D4AQDABCCMDcCwAocVoFYGKSAzFATBAMIQDeSElUeIAHFAGyq0AUAlBBVeYlXxADcAhgCcIAZwD2AWwCmAESEAXIRAC8EAEQAxAJYA7ACYBZWZsz8qMAJwtobC5ajXik/VIA2sgHQB5AK5K3gDqIrpKsgAyBrIAguIAnvoAxixScooqDlyWNiz42bxOeekKykKFfAC+SFVAA
        static async Task Main()
        {
            var someData = "FindMe";
            Console.WriteLine(1);
            await Console.Out.WriteLineAsync(someData);
            Console.WriteLine(2);
            Console.WriteLine(someData);
        }

        // https://sharplab.io/#v2:D4AQDABCCMCsDcBYAUCkBmKAmCBhCA3ihCVJiABxQBsEAIgJ4B2AhgLYCWAxgIIDOzLgAoAlBGKkiyUjJIA3FgCcIAFwgBeGgDo6AUwA2LBkOhgwIpNNmkQATghCAJs3bcxKyzIC+KL0A===
        static async Task DynamicAsync()
        {
            var t = Task.Delay(100);
            await (dynamic) t;
        }
        
        // https://sharplab.io/#v2:D4AQDABCCMDcCwAocVoFYGKSAzFATBAMIQDeSElUeIAHFAGwQByApgM4AurAJgCoAnAJ4AhADYB7AMYBrdgAoAlBApVyiKpqgBOYhIB27CWNYA6APIBXTqYDqAgJbcAMg/2sAguyH6p8gETQ/oqYWlScwqph6mFhILpEBkYmFtZ2ji5unt6+AfjBobEAvlFaAGZuAIZiYkKlmjGxmvF6hsZmVjb2Tqyu7l4+fv4ALAUqGsWlJYhFQA==
        static async Task NestedTryBlocks()
        {
            await Console.Out.WriteLineAsync("1");
            try
            {
                await Console.Out.WriteLineAsync("2");
            }
            finally
            {
                await Console.Out.WriteLineAsync("3"); 
            }
        }
    }
}