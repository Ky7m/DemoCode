using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace DebuggingScenarios.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebuggingScenarios : ControllerBase
    {
         object o1 = new object();
        object o2 = new object();

        private static Processor _p = new Processor();

        [HttpGet]
        [Route("deadlock/")]
        public ActionResult<string> Deadlock()
        {
            new Thread(() =>
            {
                DeadlockFunc();
            }).Start();

            Thread.Sleep(5000);

            var threads = new Thread[300];
            for (var i = 0; i < 300; i++)
            {
                (threads[i] = new Thread(() =>
                {
                    lock (o1) { Thread.Sleep(100); }
                })).Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            return "success:deadlock";
        }

        private void DeadlockFunc()
        {
            lock (o1)
            {
                (new Thread(() =>
                {
                    lock (o2) { Monitor.Enter(o1); }
                })).Start();

                Thread.Sleep(2000);
                Monitor.Enter(o2);
            }
        }

        [HttpGet]
        [Route("memspike/{seconds}")]
        public ActionResult<string> Memspike(int seconds)
        {
            var watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                _p = new Processor();
                watch.Stop();
                if (watch.ElapsedMilliseconds > seconds * 1000)
                    break;
                watch.Start();

                int it = (2000 * 1000);
                for (int i = 0; i < it; i++)
                {
                    _p.ProcessTransaction(new Customer(Guid.NewGuid().ToString()));
                }

                Thread.Sleep(5000); // Sleep for 5 seconds before cleaning up

                // Cleanup
                _p = null;

                // GC
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                Thread.Sleep(5000); // Sleep for 5 seconds before spiking memory again
            }
            return "success:memspike";
        }

        [HttpGet]
        [Route("memleak/{kb}")]
        public ActionResult<string> Memleak(int kb)
        {
            int it = (kb * 1000) / 100;
            for (int i = 0; i < it; i++)
            {
                _p.ProcessTransaction(new Customer(Guid.NewGuid().ToString()));
            }

            return "success:memleak";
        }

        [HttpGet]
        [Route("exception")]
        public ActionResult<string> Exception()
        {
            throw new Exception("bad, bad code");
        }

        [HttpGet]
        [Route("highcpu/{milliseconds}")]
        public ActionResult<string> Highcpu(int milliseconds)
        {
            var watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                watch.Stop();
                if (watch.ElapsedMilliseconds > milliseconds)
                    break;
                watch.Start();
            }

            return "success:highcpu";
        }
    }

    class Customer
    {
        public string Id { get; set; }

        public Customer(string id)
        {
            Id = id;
        }
    }

    class CustomerCache
    {
        public readonly List<Customer> Cache = new List<Customer>();

        public void AddCustomer(Customer c)
        {
            Cache.Add(c);
        }
    }

    class Processor
    {
        private readonly CustomerCache _cache = new CustomerCache();

        public void ProcessTransaction(Customer customer)
        {
            _cache.AddCustomer(customer);
        }
    }
}