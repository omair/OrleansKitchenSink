using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Serilog;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostConfiguration, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(hostConfiguration.Configuration);
                    var logger = loggerConfiguration.CreateLogger();
                    loggingBuilder.AddSerilog(logger);
                }).UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .AddMemoryGrainStorage(name: "HelloGrainStorage")
                        .AddMemoryGrainStorage(name: "DeviceGrainStorage")
                        .AddLogStorageBasedLogConsistencyProvider("LogStorage");

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
;

    }
}
