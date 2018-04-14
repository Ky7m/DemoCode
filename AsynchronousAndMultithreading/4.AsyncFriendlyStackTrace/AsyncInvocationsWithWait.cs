﻿using System;
using System.Threading.Tasks;

namespace AsyncFriendlyExceptionStackTrace
{
    public class AsyncInvocationsWithWait
    {
        public async Task Run()
        {
            await A();
        }

        private async Task A()
        {
            B().Wait();
            await Task.Yield();
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