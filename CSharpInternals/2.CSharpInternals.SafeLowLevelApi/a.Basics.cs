using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using DemoCode.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.SafeLowLevelApi
{
    public class Basics : BaseTestHelpersClass
    {
        public Basics(ITestOutputHelper output) : base(output) { }
       
        [Fact]
        public void SafeCodeBreaking()
        {
            var methodToPathHandle = GetMethodHandle(nameof(MethodToPatch));
            
            RuntimeHelpers.PrepareMethod(methodToPathHandle);
          
            //System.Runtime.InteropServices.Marshal.WriteByte(methodToPathHandle.GetFunctionPointer(), 0xCC);
            
            MethodToPatch();
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void MethodToPatch()
        {
            WriteLine($"{nameof(MethodToPatch)} was called.");
        }
        
        private static RuntimeMethodHandle GetMethodHandle(string methodName) => typeof(Basics)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).MethodHandle;
    }
}