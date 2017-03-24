using System;
using static System.Console;

namespace CSharp7
{
    class ThrowExpressions
    {
        public ThrowExpressions(string param)
        {
            var val = param ?? throw new ArgumentNullException(nameof(param));

            _ = param ?? throw new ArgumentNullException(nameof(param));

            WriteLine(val);
        }
    }
}
