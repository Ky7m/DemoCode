using System;

namespace NewInTheDotNETFramework
{
    internal class CompatibilitySwitches
    {
        public static void PerformAnOperation()
        {
            //AppContext.SetSwitch("Switch.ThrowOnException", true);

            bool shouldThrow;
            if (!AppContext.TryGetSwitch("Switch.ThrowOnException", out shouldThrow))
            {
                shouldThrow = true;
            }

            if (shouldThrow)
            {
                // old code
            }
            else
            {
                // new code
            }
        }
    }
}
