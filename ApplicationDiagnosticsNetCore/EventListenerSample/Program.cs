using System;

namespace EventListenerSample
{
    static class Program
    {
        static void Main(string[] args)
        {
            const int amount = 500000;

            using (new SimpleGCEventListener())
            {
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.WriteLine("Started!" + Environment.NewLine);
                
                GC.TryStartNoGCRegion(8 * amount + 50 * amount);

                var tonsOfObjects = new object[amount];
                for (var i = 0; i < amount; i++)
                {
                    tonsOfObjects[i] = new byte[8];
                    if (i % 10000 == 0)
                    {
                        tonsOfObjects[i] = new byte[85000]; // candidate for LOH
                    }
                }

                GC.EndNoGCRegion();

                Console.WriteLine("Finished!");
                Console.ReadLine();
            }
        }
    }
}