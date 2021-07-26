using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Sample.Console1
{
    public class PrintTimeService : BackgroundService
    {
        public PrintTimeService(IConfiguration configuration, ILogger<PrintTimeService> logger)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConfiguration Configuration { get; }
        public ILogger<PrintTimeService> Logger { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(DateTime.Now.ToString("s"));
                this.Logger.LogInformation("Microsoft.Extensions.Logging {ot}", this.Configuration.GetValue<string>("ot"));
                Log.Logger.Information("Serilog.Log.Logger {dt}", this.Configuration.GetValue<string>("dt"));

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
