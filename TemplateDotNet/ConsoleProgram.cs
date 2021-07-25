using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace TemplateDotNet
{
    public abstract class ConsoleProgram
    {
        public int Run(string[] args)
        {
            Log.Logger = this.SerilogStartupConfiguration().CreateLogger();

            try
            {
                Log.Logger.Information("Starting host");

                HostBuilder builder = new HostBuilder();

                builder.ConfigureHostConfiguration(confBuilder => this.ConfigureHostConfiguration(confBuilder, args))
                .ConfigureAppConfiguration((context, confBuilder) => this.ConfigureAppConfiguration(context, confBuilder, args))
                .ConfigureServices(this.ConfigureServices)
                .UseSerilog(this.SerilogConfiguration);

                builder.UseConsoleLifetime().Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        protected virtual LoggerConfiguration SerilogStartupConfiguration()
        {
            return new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console();
        }

        protected virtual void SerilogConfiguration(HostBuilderContext context, IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration)
        {

        }

        protected virtual void ConfigureHostConfiguration(IConfigurationBuilder builder, string[] args)
        {
            builder.AddEnvironmentVariables().AddCommandLine(args);
        }

        protected abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);            

        protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            
        }
    }
}
