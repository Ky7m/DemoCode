//using System;
//using System.IO;

//namespace EffectiveMemoryManagement
//{
//    static class IneffectiveMemoryUsage
//    {
//        static void Main(string[] args)
//        {
//            var candidateToLargeObjectHeap = new long[85000 / 8];
//            var maxGen = GC.MaxGeneration;
//            var objectGen = GC.GetGeneration(candidateToLargeObjectHeap);
//            Console.WriteLine($"ObjectSize: {GetObjectSize(candidateToLargeObjectHeap)}");
//            Console.WriteLine($"{nameof(maxGen)}: {maxGen}");
//            Console.WriteLine($"{nameof(objectGen)}: {objectGen}");
//        }

//        private static int GetObjectSize(object testObject)
//        {
//            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//            using (var ms = new MemoryStream())
//            {
//                bf.Serialize(ms, testObject);
//                return ms.ToArray().Length;
//            }
//        }
//    }
//}