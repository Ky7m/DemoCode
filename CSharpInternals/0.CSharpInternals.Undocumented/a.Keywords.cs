using System;
using CSharpInternals.Utils;
using Xunit;
using Xunit.Abstractions;

namespace CSharpInternals.Undocumented
{
    public class Keywords : BaseTestHelpersClass
    {
        public Keywords(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ArglistAndParams()
        {
            WithArglist(__arglist(2, 3, true, "string"));
            WithParams(2, 3, true, "string");
        }
        
        [Fact]
        public void RefKeywords()
        {
            double value = 10;
            TypedReference tr = __makeref(value); // tr = &value;
            
            WriteLine( __refvalue(tr, double));
            
            __refvalue(tr, double) = 11; // *tr = 11
            WriteLine( __refvalue(tr, double));
            
            Type type = __reftype(tr); // value.GetType()
            WriteLine(type.Name);
        }

        [Fact]
        public void MakeRefUsageSample()
        {
            // reflection
            const decimal expected = 199.99m;
            
            var book = new BookStructType {title = "C# 5.0 All-in-One For Dummies", price = 19.99m};
            var fieldInfo = typeof(BookStructType).GetField("price");
            fieldInfo?.SetValue(book, expected);
            
            Assert.NotEqual(expected, book.price);
            
            // __makeref
            TypedReference reference = __makeref(book);
            fieldInfo?.SetValueDirect(reference, expected);
            
            Assert.Equal(expected, book.price);
        }

        // [CS0610] Field or property cannot be of type 'TypedReference'
        // Because a TypedReference can contain the address of a stack-allocated variable, 
        // we’re not allowed to store them in fields, same as we’re not allowed to make a field of ref type. 
        // That way CLR knows we’re never storing a reference to a “dead” stack variable.
        // private TypedReference _typedReference;
        private void WithArglist(__arglist)
        {
            WriteLine(nameof(WithArglist));

            var argIterator = new ArgIterator(__arglist);
            DumpArgs(argIterator);

            WriteLine();
            
            void DumpArgs(ArgIterator args)
            {
                while(args.GetRemainingCount() > 0)
                {
                    TypedReference tr = args.GetNextArg();
                    var obj = TypedReference.ToObject(tr);
                    var type = TypedReference.GetTargetType(tr);
                    WriteLine($"{obj} / {type}");
                }
            }
        }
      
        private void WithParams(params object[] args)
        {
            WriteLine(nameof(WithParams));
            foreach (var arg in args)
            {
                WriteLine($"{arg} / {arg.GetType()}");
            }
            WriteLine();
        }
    }

    internal struct BookStructType
    {
        public string title;
        public decimal price;
    }
}