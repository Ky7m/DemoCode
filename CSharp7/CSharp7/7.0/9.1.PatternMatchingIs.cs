using static System.Console;

namespace CSharp7
{
    public sealed class PatternMatchingIs
    {
        public PatternMatchingIs()
        {
            var i = 5;
            PrintIfFive(i);
            PrintIfInt(i);
        }
        void PrintIfFive(object input)
        {
            if (input is 5)
            {
                WriteLine("It is 5");
            }
        }

        void PrintIfInt(object input)
        {
            if (input is int i)
            {
                WriteLine(i);
            }

            
            if (!(input is int notInt))
            {
                //WriteLine(notInt.GetType());
            }
            else
            {
                WriteLine(notInt.GetType());
            }
            
        }
        
        const int Five = 5;

        bool M1(object o) => o is Five;
        bool M2(object o) => o is (Five);
   
    }
    class Five { }
}
