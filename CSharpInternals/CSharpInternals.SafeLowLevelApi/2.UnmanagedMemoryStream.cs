using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Xunit;

namespace CSharpInternals.SafeLowLevelApi
{
    [UsedImplicitly]
    public class UnmanagedMemoryStream 
    {
        [Fact]
        public void ReadWriteStruct()
        {
            const int capacity = 100;
            var inStruct = new TestStruct
            {
                int1 = 1, 
                int2 = 2, 
                bool1 = false,   
                char1 = 'p', 
                bool2 = true 
            };

            using (var buffer = new HGlobalSafeBuffer(capacity))
            using (var stream = new UnmanagedMemoryAccessor(buffer, 0, capacity, FileAccess.ReadWrite))
            {
                stream.Write(0, ref inStruct);
                stream.Read(0, out TestStruct outStruct);
                
                Assert.Equal(inStruct.int1, outStruct.int1);
                Assert.Equal(inStruct.int2, outStruct.int2);
                Assert.Equal(inStruct.bool1, outStruct.bool1);
                Assert.Equal(inStruct.char1, outStruct.char1);
                Assert.Equal(inStruct.bool2, outStruct.bool2);
            }
        }

        private struct TestStruct
        {
            public int int1;
            public bool bool1;
            public int int2;
            public char char1;
            public bool bool2;
        }

        private sealed class HGlobalSafeBuffer : SafeBuffer
        {
            internal HGlobalSafeBuffer(int capacity) : base(true)
            {
                SetHandle(Marshal.AllocHGlobal(capacity));
                Initialize((ulong)capacity);
            }

            protected override bool ReleaseHandle()
            {
                Marshal.FreeHGlobal(handle);
                return true;
            }
        }
    }
}