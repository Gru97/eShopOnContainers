using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.DocumentProjector;

namespace Ordering.DocumentProjector
{
        public class TaskRunner : IHostedService, IDisposable
        {
            private int executionCount = 0;
            private readonly ILogger<TaskRunner> logger;
            private Timer timer;
            private readonly IEventProjector eventProjector;

            public TaskRunner(ILogger<TaskRunner> logger, IEventProjector eventProjector)
            {

                this.eventProjector=  eventProjector;
                this.logger = logger;
            }

            public async Task StartAsync(CancellationToken stoppingToken)
            {
                logger.LogInformation("Timed Hosted Service running.");
                while (true)
                {
                    await eventProjector.Project();
                    //Thread.Sleep(10000);
                    //return Task.CompletedTask;

            }

        }

            private async Task DoWork(object state)
            {

                //logger.LogInformation();
            }

            public Task StopAsync(CancellationToken stoppingToken)
            {
                logger.LogInformation("Timed Hosted Service is stopping.");

                timer?.Change(Timeout.Infinite, 0);

                return Task.CompletedTask;
            }

            public void Dispose()
            {
                timer?.Dispose();
            }
        }
    }

