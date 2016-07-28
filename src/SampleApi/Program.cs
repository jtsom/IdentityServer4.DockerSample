﻿using System;
using Microsoft.AspNetCore.Hosting;

namespace SampleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://0.0.0.0:1773")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
