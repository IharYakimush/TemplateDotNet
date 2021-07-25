using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TemplateDotNet
{
    public abstract class ConsoleStartup
    {
        public abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);
    }
}
