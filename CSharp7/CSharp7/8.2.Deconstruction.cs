using System.Collections.Generic;
using static System.Console;

namespace CSharp7
{
    class Deconstruction
    {
        public Deconstruction()
        {
            var (x, y) = new Point(1, 2);

            WriteLine($"{x},{y}");

            var dictionary = new Dictionary<string, int>();
            foreach (var (key, value) in dictionary)
            {
                WriteLine($"{key} => {value}");
            }

        }

        struct Point
        {
            public double X { get; }
            public double Y { get; }

            public Point(int x, int y) { X = x; Y = y; }

            public void Deconstruct(out double x, out double y) { x = X; y = Y; }
        }

        class ServiceC
        {
            private IServiceA _serviceA;
            private IServiceB _serviceB;

            public ServiceC(IServiceA serviceA, IServiceB serviceB) =>
                (_serviceA, _serviceB) = (serviceA, serviceB);
        }


        interface IServiceA { }
        interface IServiceB { }
    }

    public static class KVPExtensions
    {
        public static void Deconstruct<TKey, TValue>(
            this KeyValuePair<TKey, TValue> kvp,
            out TKey key,
            out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
