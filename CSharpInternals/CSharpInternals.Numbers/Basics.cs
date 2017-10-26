using Xunit;

namespace CSharpInternals.Numbers
{
    public class Basics
    {
        [Fact]
        public void NearlyEqual()
        {
            Assert.NotEqual(0.3, 0.1 + 0.2);
        }
        
        [Fact]
        public void Equal()
        {
            Assert.Equal(.3m, .1m + .2m);
        }
    }
}