using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TemplateDotNet;

namespace Sample.Console1
{
    public class Startup : ConsoleStartup
    {        
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                Console.WriteLine(context.HostingEnvironment.EnvironmentName);
            }
            else if (context.HostingEnvironment.IsStaging())
            {
                Console.WriteLine(context.HostingEnvironment.EnvironmentName);
            }
            else
            {
                Console.WriteLine($"Not dev or staging. {context.HostingEnvironment.EnvironmentName}");
            }

            foreach (var item in context.Configuration.AsEnumerable())
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }

            services.AddHostedService<PrintTimeService>();
        }
    }
}
