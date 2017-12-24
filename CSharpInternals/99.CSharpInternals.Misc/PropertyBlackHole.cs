using Xunit;

namespace CSharpInternals.Misc
{
    public class PropertyBlackHole
    {
        // https://sharplab.io/#v2:CYLg1APgAgDABFAjAbgLACgoGYECY4DCcA3hnOQjgJYB2ALnAAoBOA9gA4lkU88DOAUwYBeAHxwA+nGFwAbgEMANgFcBadL01wA5kOnipMxDHVa4AXwzmgA=
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