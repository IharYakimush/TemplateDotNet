using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Sample.Console1
{
    public class PrintTimeService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(DateTime.Now.ToString("s"));
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
