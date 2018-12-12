﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HolidayPartySF2018AspNetCore22
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}