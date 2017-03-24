using System;
using System.Collections.Generic;
using static CSharp7.CacheManager;

namespace CSharp7
{
    class OutVariablesAndDiscards
    {
        void OnlyOut(out int a, out int b, out int c)
        {
            a = b = c = 5;
        }

        public OutVariablesAndDiscards()
        {
            // out vars
            int.TryParse("5", out int i);
            double.TryParse("5", out var d);

            decimal.TryParse("5", out _);

            if (Cache.TryGetValue("sum", out var fromCache))
            {
                i = fromCache;
            }
            fromCache = 10;

            // discards
            _ = 5;

            OnlyOut(out _, out _, out _);

            Func<int,int> func = _ => _;
        }
    }

    class CacheManager
    {
        public static Dictionary<string, int> Cache =
            new Dictionary<string, int>
            {
                ["sum"] = 5
            };
    }
}
