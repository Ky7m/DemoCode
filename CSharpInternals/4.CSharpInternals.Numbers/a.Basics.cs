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
        
        [Fact]
        public void SuspiciousComparison()
        {
            byte b = 1;
            int i = 1;
            Assert.Equal(b, i);
            Assert.False(b.Equals(i));
        }
    }
}