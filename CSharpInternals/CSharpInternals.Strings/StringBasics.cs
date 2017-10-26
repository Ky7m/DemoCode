using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Strings
{
    public class StringBasics : BaseTestHelpersClass
    {
        private const string Emojis = "👶👶👶👶🍼🍼";

        public StringBasics(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void StringLength()
        {
            //what do you think will be written?
            WriteLine($"Length: {Emojis.Length}"); 
        }
        
        [Fact]
        public void StringLengthInTextElements()
        {
            var stringInfo = new System.Globalization.StringInfo(Emojis);
            WriteLine($"LengthInTextElements: {stringInfo.LengthInTextElements}");
        }
    }
}