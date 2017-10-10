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
            Output.WriteLine(nameof(WithArglist));
            DumpArgs(new ArgIterator(__arglist));
            Output.WriteLine(string.Empty);
            
            void DumpArgs(ArgIterator args)
            {
                while(args.GetRemainingCount() > 0)
                {
                    TypedReference tr = args.GetNextArg();
                    var arg = TypedReference.ToObject(tr);
                    Output.WriteLine(arg.ToString());
                }
            }
        }
        
        private void WithParams(params object[] args)
        {
            Output.WriteLine(nameof(WithParams));
            foreach (var arg in args)
            {
                Output.WriteLine(arg.ToString());
            }
            Output.WriteLine(string.Empty);
        }
    }
}