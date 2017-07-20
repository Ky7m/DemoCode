using System;

namespace CSharp7
{
    public sealed class NumericLiteralSyntaxImprovements
    {
        public NumericLiteralSyntaxImprovements()
        {
            int bin = 0b1001_1010_0001_0100;
            int hex = 0x1b_a0_44_fe;
            int dec = 33_554_432;
            int weird = 1_2__3___4____5_____6______7_______8______9;
            double real = 1_000.111_1e-1_000;
        }
    }

    [Flags]
    enum RenderType
    {
        None = 0x0,
        DataUri = 0x1,
        GZip = 0x2,
        ContentPage = 0x4,
        ViewPage = 0x8,
        HomePage = 0x10
    }

    [Flags]
    enum RenderTypeNew
    {
        None = 0b00000,
        DataUri = 0b00001,
        GZip = 0b00010,
        ContentPage = 0b00100,
        ViewPage = 0b01000,
        HomePage = 0b10000
    }
}
