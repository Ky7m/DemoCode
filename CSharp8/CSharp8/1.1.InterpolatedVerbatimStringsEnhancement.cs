using Xunit;

namespace CSharp8
{
    public class InterpolatedVerbatimStringsEnhancement
    {
        [Fact]
        public void OrderOfTokensCanBeAny()
        {
            var order = "order";
            
            var correctOrderBeforeCSharp8 = $@"I always forget correct {order}";
            var correctOrderWithCSharp8 = @$"I always forget correct {order}";
            
            Assert.Equal(correctOrderBeforeCSharp8,correctOrderWithCSharp8);
        }
    }
}