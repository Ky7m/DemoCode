// http://stackoverflow.com/questions/43348128/reflection-how-do-i-find-and-invoke-a-local-functon-in-c-sharp-7-0/
using System;
using System.Reflection;

namespace CSharp7
{
    public sealed class LocalFunctionsFindAndInvoke
    {
        public LocalFunctionsFindAndInvoke()
        {
            HandleResponse("foo", typeof(string));
        }

        void HandleResponse(object data, Type type)
        {
            var local = "This was a local variable";
            void UseAs<T>(T obj)
            {
                Console.WriteLine($"Object is now a: {typeof(T)}");
                // Proof that we're capturing the target too
                Console.WriteLine($"Local was {local}");
            }

            InvokeHelper(UseAs, data, type);
        }

        /*
         It involves creating a delegate from method with a specific type, 
         then using that to find the generic method, 
         then constructing another specific method and invoking it.
         */
        void InvokeHelper(Action<int> int32Action, object data, Type type)
        {
            // You probably want to validate that it really is a generic method...
            var method = int32Action.GetMethodInfo();
            var genericMethod = method.GetGenericMethodDefinition();
            var concreteMethod = genericMethod.MakeGenericMethod(type);
            concreteMethod.Invoke(int32Action.Target, new[] { data });
        }
    }
}
