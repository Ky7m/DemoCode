//using System;
//using System.Collections.Generic;

//namespace EffectiveMemoryManagement
//{
//    static class ResizingCollections
//    {
//        static void Main(string[] args)
//        {
//            const int n = 1000000;
//            var capacities = new HashSet<long>();
//            var list = new List<int>();
//            for (var i = 0; i < n; i++)
//            {
//                capacities.Add(list.Capacity);
//                list.Add(i);
//            }
//            Console.WriteLine($"number of resizes: {capacities.Count} \n");
//            Console.WriteLine("Resizes:");
//            foreach (var capacity in capacities)
//            {
//                Console.WriteLine(capacity);
//            }
//        }
//    }
//}