using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSiteHealthCheckUp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient httpClient = null;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await httpClient.GetAsync("https://www.google.com");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status Code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("The Website is down. Status Code {StatusCode}",result.StatusCode);
                    
                    // Write Addition code
                    // Send Mail to Admin.
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
