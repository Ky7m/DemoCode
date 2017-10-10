using System;
using CSharpInternals.Base;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals
{
    public sealed class UndocumentedKeywords : BaseTestHelpersClass
    {
        public UndocumentedKeywords(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ArglistAndParamsSample()
        {
            WithArglist(__arglist(2, 3, true, "string"));
            WithParams(2, 3, true, "string");
        }
        
        private void WithArglist(__arglist)
        {
            WriteLine(nameof(WithArglist));

            var argIterator = new ArgIterator(__arglist);
            DumpArgs(argIterator);

            WriteLine(string.Empty);
            
            void DumpArgs(ArgIterator args)
            {
                while(args.GetRemainingCount() > 0)
                {
                    TypedReference tr = args.GetNextArg();
                    var arg = TypedReference.ToObject(tr);
                    WriteLine(arg.ToString());
                }
            }
        }
        
        private void WithParams(params object[] args)
        {
            WriteLine(nameof(WithParams));
            foreach (var arg in args)
            {
                WriteLine(arg.ToString());
            }
            WriteLine(string.Empty);
        }
    }
}