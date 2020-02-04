using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NHibernate;
using Serilog;
using System;
using System.Linq;

namespace Alibi.Framework.Startup
{
    public static class FrameworkBoot
    {
        public static void Start(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            NHibernateLogger.SetLoggersFactory(new NHibernate.Logging.Serilog.SerilogLoggerFactory());

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();

                // Host.CreateDefaultBuilder().Run<Startup>();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var type = typeof(FrameworkStartupBase);
                    var customStartup = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(s => s.GetTypes()).FirstOrDefault(p =>
                            p.FullName != null &&
                            (type.IsAssignableFrom(p) && !p.FullName.ToLower().Contains("alibi")));

                    if (customStartup != null)
                    {
                        webBuilder.UseStartup(customStartup);
                    }
                    else
                    {
                        webBuilder.UseStartup<FrameworkStartup>();
                    }
                });
    }
}