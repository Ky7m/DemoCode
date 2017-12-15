using System;
using System.Globalization;
using System.Text;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Strings
{
    public class StringBasics : BaseTestHelpersClass
    {
        public StringBasics(ITestOutputHelper output) : base(output) { }
        
        [Fact]
        [ಠ_ಠ]
        public void UnicodeIsAwesomeForCoding()
        {
            const double λ = 1.4;
            const double ξ = 0.00000000000001;
            
            try
            {
                var Ʃ = λ * 1.5 + ξ;
                WriteLine(Ʃ);
                throw new Exception("💩");
            }
            catch(Exception ಠ_ಠ)
            {
                WriteLine(ಠ_ಠ.Message);
            }
        }
        
        [Fact]
        public void EmojiEncoding()
        {
            const string expectedDecoded = "rf^4Ke";
            const string expectedEncoded = "🚺😁🚤😁🚑😁🙊😁❎😁🚢😁";
            
            var encoded = EmojiEncoder.Encode(expectedDecoded);
            Assert.Equal(expectedEncoded, encoded);
            var decoded = EmojiEncoder.Decode(encoded);
            Assert.Equal(expectedDecoded, decoded);
        }

        private const string Emojis = "🦆🦆🦆🦆";
        
        [Fact]
        public void StringLength()
        {
            //what do you think will be written?
            WriteLine($"Length: {Emojis.Length}"); 
        }
        
        [Fact]
        public void StringLengthInTextElements()
        {
            var stringInfo = new StringInfo(Emojis);
            WriteLine($"LengthInTextElements: {stringInfo.LengthInTextElements}");
        }
    }
    
    // Disapprove
    public class ಠ_ಠAttribute : Attribute
    {
    }
        
    
    // https://gist.githubusercontent.com/ayende/c7977cda3fe64c1399fea80837c9904e/raw/59abc67fe40a361a58653243b0f4b00d17947eea/EmojiEncoding.cs
    class EmojiEncoder
    {
        public static string Decode(string text)
        {
            var buffer = new byte[new StringInfo(text).LengthInTextElements];

            int index = 0;
            var enumerator = StringInfo.GetTextElementEnumerator(text);
            while (enumerator.MoveNext())
            {
                buffer[index++] = (byte)Array.IndexOf(Emoji, (string)enumerator.Current);
            }
            return Encoding.Unicode.GetString(buffer);
        }
        

        public static string Encode(string text)
        {
            var buffer = Encoding.Unicode.GetBytes(text);
            var sb = new StringBuilder(buffer.Length);
            foreach (var b in buffer)
            {
                sb.Append(Emoji[b]);
            }
            return sb.ToString();
        }


        public static string[] Emoji = new[]
        {
            "\U0001F601",
            "\U0001F602",
            "\U0001F603",
            "\U0001F604",
            "\U0001F605",
            "\U0001F606",
            "\U0001F609",
            "\U0001F60A",
            "\U0001F60B",
            "\U0001F60C",
            "\U0001F60D",
            "\U0001F60F",
            "\U0001F612",
            "\U0001F613",
            "\U0001F614",
            "\U0001F616",
            "\U0001F618",
            "\U0001F61A",
            "\U0001F61C",
            "\U0001F61D",
            "\U0001F61E",
            "\U0001F620",
            "\U0001F621",
            "\U0001F622",
            "\U0001F623",
            "\U0001F624",
            "\U0001F625",
            "\U0001F628",
            "\U0001F629",
            "\U0001F62A",
            "\U0001F62B",
            "\U0001F62D",
            "\U0001F630",
            "\U0001F631",
            "\U0001F632",
            "\U0001F633",
            "\U0001F635",
            "\U0001F637",
            "\U0001F638",
            "\U0001F639",
            "\U0001F63A",
            "\U0001F63B",
            "\U0001F63C",
            "\U0001F63D",
            "\U0001F63E",
            "\U0001F63F",
            "\U0001F640",
            "\U0001F645",
            "\U0001F646",
            "\U0001F647",
            "\U0001F648",
            "\U0001F649",
            "\U0001F64A",
            "\U0001F64B",
            "\U0001F64C",
            "\U0001F64D",
            "\U0001F64E",
            "\U0001F64F",
            "\U00002702",
            "\U00002705",
            "\U00002708",
            "\U00002709",
            "\U0000270A",
            "\U0000270B",
            "\U0000270C",
            "\U0000270F",
            "\U00002712",
            "\U00002714",
            "\U00002716",
            "\U00002728",
            "\U00002733",
            "\U00002734",
            "\U00002744",
            "\U00002747",
            "\U0000274C",
            "\U0000274E",
            "\U00002753",
            "\U00002754",
            "\U00002755",
            "\U00002757",
            "\U00002764",
            "\U00002795",
            "\U00002796",
            "\U00002797",
            "\U000027A1",
            "\U000027B0",
            "\U0001F680",
            "\U0001F683",
            "\U0001F684",
            "\U0001F685",
            "\U0001F687",
            "\U0001F689",
            "\U0001F68C",
            "\U0001F68F",
            "\U0001F691",
            "\U0001F692",
            "\U0001F693",
            "\U0001F695",
            "\U0001F697",
            "\U0001F699",
            "\U0001F69A",
            "\U0001F6A2",
            "\U0001F6A4",
            "\U0001F6A5",
            "\U0001F6A7",
            "\U0001F6A8",
            "\U0001F6A9",
            "\U0001F6AA",
            "\U0001F6AB",
            "\U0001F6AC",
            "\U0001F6AD",
            "\U0001F6B2",
            "\U0001F6B6",
            "\U0001F6B9",
            "\U0001F6BA",
            "\U0001F6BB",
            "\U0001F6BC",
            "\U0001F6BD",
            "\U0001F6BE",
            "\U0001F6C0",
            "\U000024C2",
            "\U0001F170",
            "\U0001F171",
            "\U0001F17E",
            "\U0001F17F",
            "\U0001F18E",
            "\U0001F191",
            "\U0001F192",
            "\U0001F193",
            "\U0001F194",
            "\U0001F195",
            "\U0001F196",
            "\U0001F197",
            "\U0001F198",
            "\U0001F199",
            "\U0001F19A",
            "\U0001F201",
            "\U0001F202",
            "\U0001F21A",
            "\U0001F22F",
            "\U0001F232",
            "\U0001F233",
            "\U0001F234",
            "\U0001F235",
            "\U0001F236",
            "\U0001F237",
            "\U0001F238",
            "\U0001F239",
            "\U0001F23A",
            "\U0001F250",
            "\U0001F251",
            "\U000000A9",
            "\U000000AE",
            "\U0000203C",
            "\U00002049",
            "\U00002122",
            "\U00002139",
            "\U00002194",
            "\U00002195",
            "\U00002196",
            "\U00002197",
            "\U00002198",
            "\U00002199",
            "\U000021A9",
            "\U000021AA",
            "\U0000231A",
            "\U0000231B",
            "\U000023E9",
            "\U000023EA",
            "\U000023EB",
            "\U000023EC",
            "\U000023F0",
            "\U000023F3",
            "\U000025AA",
            "\U000025AB",
            "\U000025B6",
            "\U000025C0",
            "\U000025FB",
            "\U000025FC",
            "\U000025FD",
            "\U000025FE",
            "\U00002600",
            "\U00002601",
            "\U0000260E",
            "\U00002611",
            "\U00002614",
            "\U00002615",
            "\U0000261D",
            "\U0000263A",
            "\U00002648",
            "\U00002649",
            "\U0000264A",
            "\U0000264B",
            "\U0000264C",
            "\U0000264D",
            "\U0000264E",
            "\U0000264F",
            "\U00002650",
            "\U00002651",
            "\U00002652",
            "\U00002653",
            "\U00002660",
            "\U00002663",
            "\U00002665",
            "\U00002666",
            "\U00002668",
            "\U0000267B",
            "\U0000267F",
            "\U00002693",
            "\U000026A0",
            "\U000026A1",
            "\U000026AA",
            "\U000026AB",
            "\U000026BD",
            "\U000026BE",
            "\U000026C4",
            "\U000026C5",
            "\U000026CE",
            "\U000026D4",
            "\U000026EA",
            "\U000026F2",
            "\U000026F3",
            "\U000026F5",
            "\U000026FA",
            "\U000026FD",
            "\U00002934",
            "\U00002935",
            "\U00002B05",
            "\U00002B06",
            "\U00002B07",
            "\U00002B1B",
            "\U00002B1C",
            "\U00002B50",
            "\U00002B55",
            "\U00003030",
            "\U0000303D",
            "\U00003297",
            "\U00003299",
            "\U0001F004",
            "\U0001F0CF",
            "\U0001F300",
            "\U0001F301",
            "\U0001F302",
            "\U0001F303",
            "\U0001F304",
            "\U0001F305",
            "\U0001F306",
            "\U0001F307",
            "\U0001F308",
            "\U0001F309",
            "\U0001F30A",
            "\U0001F30B",
            "\U0001F30C",
            "\U0001F30F",
            "\U0001F311",
            "\U0001F313",
        };

    }
}