using Xunit;
#pragma warning disable 8321

namespace CSharp8
{
    public sealed class StaticLocalFunctions
    {
        private static int bb = 1;
        
        [Fact]
        public void UseLocalFunctions()
        {
            int aa = 5;
            var result = LocalAdd();
            Assert.Equal(aa + bb, result);
            
            int LocalAdd() => aa + bb;
            
            static int StaticLocalAdd(int a) => a + bb;
        }
    }
}