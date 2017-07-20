using System;
using static System.Console;

namespace CSharp7
{
    public sealed class RefLocalsAndReturnsPart1
    {
        public RefLocalsAndReturnsPart1(bool intro)
        {
            var num1 = 0;
            var num2 = 5;

            int max1 = GetMaxByValue(num1, num2);
            int max2 = GetMaxByRef(ref num1, ref num2);

            WriteLine($"max1={max1},max2={max2}");
        }

        int GetMaxByValue(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        ref int GetMaxByRef(ref int a, ref int b)
        {
            if (a > b)
            {
                return ref a;
            }
            return ref b;
        }

        public RefLocalsAndReturnsPart1()
        {
            var array = new[] { 1, 2, 3 };
            ref var thirdElement = ref array[2];

            thirdElement = 5;

            WriteLine($"thirdElement= {thirdElement}");
            WriteLine($"before resize: {string.Join(",", array)}");

            Array.Resize(ref array, 1);

            WriteLine($"after resize: {string.Join(",", array)}");
            WriteLine($"thirdElement= {thirdElement}");

            //array.SetValue(2,3);
        }
    }
}
