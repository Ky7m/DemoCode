using System.Collections.Generic;
using static System.Console;

namespace CSharp7
{
    class RefLocalsAndReturnsPart2
    {
        public ref int RefLocalsAndReturns(out int x)
        {
            ref int val = ref Find(1, new[] { 1, 2, 3 });
            
            // it works only with arrays
            var numbers = new List<int> { 1 };
            //ref int second = ref numbers[0];
            
            
            var list = new OptimizedList<Point>();
            list[0].X = 5;
            
            int a = 0;
            ref var b = ref a;
            b = 5;
            WriteLine($"a={a},b={b},val={val}");

            Find(1, new[] { 1 }) += 10;

            return ref RefLocalsAndReturns(out x); // b _
        }

        ref int Find(int number, int[] numbers) => ref numbers[0];

        class OptimizedList<T>
        {
            private T[] _items;
            // … The rest of List<T>’s implementation
            public ref T this[int index] => ref _items[index];
        }

        struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
