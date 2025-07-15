using Jumia_Api.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jumia_Api.Infrastructure.External_Services
{
    public class CampaignEmailWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CampaignEmailWorker> _logger;

        public CampaignEmailWorker(IServiceProvider serviceProvider, ILogger<CampaignEmailWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CampaignEmailWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var campaignService = scope.ServiceProvider.GetRequiredService<ICampaignEmailService>();

                try
                {
                    await campaignService.ProcessPendingCampaignsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing marketing campaigns.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("CampaignEmailWorker stopped.");
        }
    }
}
