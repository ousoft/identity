using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace Oyang.Identity.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, services, loggerConfiguration) =>
                {
                    var options = new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions();
                    options.AutoCreateSqlTable = true;
                    options.TableName = "T_Log";

                    loggerConfiguration
                        .ReadFrom
                        .Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Debug()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                        //.WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/log_.txt", rollingInterval: RollingInterval.Day)
                        .WriteTo.MSSqlServer("Data Source=localhost;Initial Catalog=TestDB;User ID=app;Password=123", options)
                        .WriteTo.Logger(t => t.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/debug_.txt", rollingInterval: RollingInterval.Day))
                        .WriteTo.Logger(t => t.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/information_.txt", rollingInterval: RollingInterval.Day))
                        .WriteTo.Logger(t => t.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/warning_.txt", rollingInterval: RollingInterval.Day))
                        .WriteTo.Logger(t => t.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/error_.txt", rollingInterval: RollingInterval.Day))
                        .WriteTo.Logger(t => t.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File($"logs/{DateTime.Now.ToString("yyyy-MM")}/fatal_.txt", rollingInterval: RollingInterval.Day));
                });
    }
}
