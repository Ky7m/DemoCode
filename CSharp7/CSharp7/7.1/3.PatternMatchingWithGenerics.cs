using System;
using static System.Console;

namespace CSharp7
{
    public sealed class PatternMatchingWithGenerics
    {
        public PatternMatchingWithGenerics()
        {
            WriteLine(M1<int>(2));
            WriteLine(M2<int>(3));
            WriteLine(M1<int>(1.1));
            WriteLine(M2<int>(1.1));
        }

        public static T M1<T>(ValueType o)
        {
            return o is T t ? t : default;
        }
        public static T M2<T>(ValueType o)
        {
            switch (o)
            {
                case T t:
                    return t;
                case ValueTuple<int,double> _:
                    return default;
                default:
                    return default;
            }
        }
    }
}
