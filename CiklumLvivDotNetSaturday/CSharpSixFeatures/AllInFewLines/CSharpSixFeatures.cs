using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CSharpSixFeatures.Helpers;

namespace CSharpSixFeatures
{
    public class CSharpSixFeatures
    {
        private static int AutoProp(int x) => x + 5;
        public static Dictionary<string, Task<string>> Exp { get; } = new Dictionary<string, Task<string>>
        {
            ["A"] = Task.Run(async () =>
            {
                try { return $"{{ \"{nameof(AutoProp)}\": { await A()?.B(AutoProp(1))}  }}"; }
                catch (Exception e) when (e.HResult == 1) { return null; }
                finally { await "".B(4); }
            })
        };
    }
}