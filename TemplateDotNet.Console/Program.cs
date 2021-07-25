using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TemplateDotNet.ConsoleHost
{
    class Program : ConsoleProgram
    {
        private const string AppKey = "app";

        public static int Main(string[] args)
        {
            return new Program().Run(args);           
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            this.ConfigureCommonServices(context, services);

            string app = context.Configuration.GetValue<string>(AppKey);

            if (app == null)
            {
                throw new InvalidOperationException($"Application name to run should be provided in '{AppKey}' setting for host configuration");
            }

            ConsoleStartup startup = this.GetStartup(app);

            startup.ConfigureServices(context, services);
        }

        private void ConfigureCommonServices(HostBuilderContext context, IServiceCollection services)
        {

        }

        private ConsoleStartup GetStartup(string name)
        {
            if (name == "sample1")
            {
                return new Sample.Console1.Startup();
            }

            throw new InvalidOperationException($"Startup for '{name}' not registered");
        }
    }
}
