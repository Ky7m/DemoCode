//https://github.com/dotnet/roslyn/issues/16670

using System;
using System.Runtime.CompilerServices;

namespace CSharp7
{
    public sealed class RefLocalsAndReturnsPart3
    {
        public RefLocalsAndReturnsPart3()
        {
            var array = new[] { 1, 2, 3 };

            {
                ref var e = ref FindValue(array, 4);
                if (!IsNull(ref e)) e = 5;
            }

            {
                ref var e = ref FindValue(array, 3);
                if (!IsNull(ref e))
                {
                    e = 0;
                }
            }

            foreach (var x in array)
            {
                Console.WriteLine(x);
            }
        }

        static ref int FindValue(int[] arr, int value)
        {
            int i;
            for (i = 0; i < arr.Length; i++)
            {
                if (value == arr[i]) return ref arr[i];
            }
            return ref NullRef<int>();
        }

        private static unsafe ref T NullRef<T>() where T : struct => ref Unsafe.AsRef<T>((void*)0);
        private static unsafe bool IsNull<T>(ref T r) where T : struct => Unsafe.AsPointer(ref r) == (void*)0;
    }
}
