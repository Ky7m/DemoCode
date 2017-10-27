using System;
using System.IO;
using System.Runtime.InteropServices;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.SafeLowLevelApi
{
    public class SystemUnmanagedMemoryStream :BaseTestHelpersClass
    {
        private const int Capacity = 1073741824; //1GB
        public SystemUnmanagedMemoryStream(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ReadWriteStructUsingUma()
        {
            
            var inStruct = new TestStruct
            {
                int1 = 1, 
                int2 = 2, 
                bool1 = false,   
                char1 = 'p', 
                bool2 = true 
            };

            var startMemory = GC.GetTotalMemory(true);
            using (var buffer = new HGlobalSafeBuffer(Capacity))
            using (var stream = new UnmanagedMemoryAccessor(buffer, 0, Capacity, FileAccess.ReadWrite))
            {
                stream.Write(0, ref inStruct);
                stream.Read(0, out TestStruct outStruct);

                Assert.Equal(inStruct.int1, outStruct.int1);
                Assert.Equal(inStruct.int2, outStruct.int2);
                Assert.Equal(inStruct.bool1, outStruct.bool1);
                Assert.Equal(inStruct.char1, outStruct.char1);
                Assert.Equal(inStruct.bool2, outStruct.bool2);

                var endMemory = GC.GetTotalMemory(false);
                WriteLine($"Memory: {endMemory - startMemory} bytes");
            }
        }

        [Fact]
        public void ReadWriteFromUnmanagedMemoryStream()
        {
            var startMemory = GC.GetTotalMemory(true);
            using (var buffer = new HGlobalSafeBuffer(Capacity))
            using (var stream = new UnmanagedMemoryStream(buffer, 0, Capacity, FileAccess.ReadWrite))
            {
                buffer.Write(0, (byte)100);

                var position = stream.Position;
                Assert.Equal(100, stream.ReadByte());
                Assert.Equal(stream.Position, position + 1);

                var endMemory = GC.GetTotalMemory(false);
                WriteLine($"Memory: {endMemory - startMemory} bytes");
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