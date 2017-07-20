using System;
using System.Collections.Generic;
using static System.Console;

namespace CSharp7
{
    public sealed class Tuples
    {
        public Tuples()
        {
            Tuple<int, int> oldTuple = new Tuple<int, int>(1, 2);
            (int, int) newTuple = (1, 2);

            //newTuple = oldTuple;
            
            var i = new ValueTuple<int, int>(1,2);
            ref (int a, int b) ab = ref i;
            ab.a = 5;

            Dictionary<(int, int), string> exampleDictionary;

            WriteLine(oldTuple);
            WriteLine(newTuple);


            int a, b;
            (a, b) = (0, 1);

            var (x, y) = (0, 1);

            var t = (1, 2, 3, 4, 5, 6, 7, 8);

            WriteLine($"{a},{b},{x},{y},{t}");
        }

        (int left, int right) GetLimits()
        {
            return (1, 2);
        }
    }
}
