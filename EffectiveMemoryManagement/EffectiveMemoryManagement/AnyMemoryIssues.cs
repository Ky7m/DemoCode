using System;

namespace EffectiveMemoryManagement
{
    static class AnyMemoryIssues
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"This OS has {GC.MaxGeneration + 1} object generations.");

            var myClass = new MyClass("First", 100);
            Console.WriteLine($"\nGeneration of myClass is: {GC.GetGeneration(myClass)}");

            var amount = 50000000;
            var tonsOfObjects = new object[amount];
            for (var i = 0; i < amount; i++)
            {
                tonsOfObjects[i] = new object();
            }

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Generation of myClass is: {GC.GetGeneration(myClass)}");
            Console.WriteLine($"\nGen 0 has been swept {GC.CollectionCount(0)} times");
            Console.WriteLine($"Gen 1 has been swept {GC.CollectionCount(1)} times");
            Console.WriteLine($"Gen 2 has been swept {GC.CollectionCount(2)} times");
        }
    }

    class MyClass
    {
        public string Str { get; }
        public int Number { get; }

        public MyClass(string str, int number)
        {
            Str = str;
            Number = number;
        }
    }
}