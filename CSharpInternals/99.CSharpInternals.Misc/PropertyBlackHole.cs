using Xunit;

namespace CSharpInternals.Misc
{
    public class PropertyBlackHole
    {
        private int Prop {
            set => _ = value;
            get => _ = 5;
        }

        [Fact]
        public void WriteToBlackHole()
        {
            const int expected = 5;
            Prop = expected;
            Assert.Equal(expected, Prop);
        }
    }
}