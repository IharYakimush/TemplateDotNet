using System;
using System.Collections.Generic;

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
            Log.Logger = this.SerilogStartupConfiguration().CreateBootstrapLogger();

            try
            {
                Log.Logger.Information("Starting host");

                HostBuilder builder = new HostBuilder();

                builder.ConfigureHostConfiguration(confBuilder => this.SetHostConfiguration(confBuilder, args))
                .ConfigureAppConfiguration((context, confBuilder) => this.SetAppConfiguration(context, confBuilder, args))
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

        protected virtual LoggerConfiguration SerilogStartupConfiguration(LoggerConfiguration loggerConfiguration = null)
        {
            return (loggerConfiguration ?? new LoggerConfiguration())
           .MinimumLevel.Debug()           
           .WriteTo.Console();
        }

        protected virtual void SerilogConfiguration(HostBuilderContext context, IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration)
        {
            this.SerilogStartupConfiguration(loggerConfiguration);
        }

        protected virtual void SetHostConfiguration(IConfigurationBuilder builder, string[] args)
        {
            foreach (var item in this.ConfigHostEnvVarPrefixes)
            {
                builder.AddEnvironmentVariables(item);
            }

            builder.AddCommandLine(args);
        }

        protected virtual IEnumerable<string> ConfigHostEnvVarPrefixes { get
            {
                yield return "DOTNET_";
            } 
        }

        protected virtual IEnumerable<string> ConfigAppJsonFiles(HostBuilderContext context)
        {
            string env = context.HostingEnvironment.EnvironmentName.ToLowerInvariant();

            yield return "sln-settings.json";
            yield return $"sln-settings.{env}.json";
            yield return "app-settings.json";
            yield return $"app-settings.{env}.json";
        }

        protected virtual IEnumerable<string> ConfigAppEnvVarPrefixes => this.ConfigHostEnvVarPrefixes;

        protected abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);            

        protected virtual void SetAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            foreach (var item in this.ConfigAppJsonFiles(context))
            {
                builder.AddJsonFile(item, true, true);
            }

            foreach (var item in this.ConfigAppEnvVarPrefixes)
            {
                builder.AddEnvironmentVariables(item);
            }

            builder.AddCommandLine(args);
        }
    }
}
