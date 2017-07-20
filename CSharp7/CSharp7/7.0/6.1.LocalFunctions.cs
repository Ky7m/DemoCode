using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp7
{
    public sealed class LocalFunctions
    {
        public LocalFunctions()
        {
            int AddLocal(int a, int b) => a + b;

            int aa = 5;
            int bb = 0;
            var result = AddLocal(aa, bb);

            int AddLocalWithClosure(int a) => a + bb;
            
            async Task<int> AddAsyncLocal(int a, int b)
            {
                await Task.Yield();
                return a + b;
            }
        }

        [MyAttr]
        public void M2([MyAttr] int a)
        {
            // AddLocal(); // it is not visible there
            // int AddLocal([MyAttr] int a, int b) => a + b; // attributes are not allowed
        }
    }

    public sealed class MyAttr : Attribute
    {
    }
}
