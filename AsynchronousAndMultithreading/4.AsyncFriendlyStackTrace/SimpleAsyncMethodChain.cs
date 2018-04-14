using System;
using System.Threading.Tasks;

namespace AsyncFriendlyExceptionStackTrace
{
    public class SimpleAsyncMethodChain
    {
        public async Task Run()
        {
            await A();
        }

        private async Task A()
        {
            await B();
        }

        private async Task B()
        {
            await C();
        }

        private async Task C()
        {
            await Task.Yield();
            throw new Exception("Crash! Boom! Bang!");
        }
    }
}