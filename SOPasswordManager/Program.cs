﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SOPasswordManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHost BuildWebHost(string[] args) // =>
        //{
        //    var config = new ConfigurationBuilder()
        //       .AddJsonFile("appsettings.json", optional: true)
        //       .AddCommandLine(args)
        //       .Build();

        //    return WebHost.CreateDefaultBuilder(args)
        //        .UseConfiguration(config)
        //        .UseStartup<Startup>()
        //        .Build();
        //}

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
