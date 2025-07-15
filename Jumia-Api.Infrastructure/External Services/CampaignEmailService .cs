using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class CampaignEmailService : ICampaignEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<CampaignEmailService> _logger;

        public CampaignEmailService(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<CampaignEmailService> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public Task ProcessPendingCampaignsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /*  public async Task ProcessPendingCampaignsAsync(CancellationToken cancellationToken = default)
          {
              var pendingCampaigns = await _unitOfWork.Campaigns.GetPendingCampaignsAsync();

              foreach (var campaign in pendingCampaigns)
              {
                  _logger.LogInformation("Processing Campaign ID: {CampaignId}", campaign.Id);
                  campaign.Status = CampaignStatus.InProgress;
                  await _unitOfWork.SaveChangesAsync();

                  var recipients = await _unitOfWork.Users.GetLoyalUsersAsync(campaign.SellerId);

                  foreach (var recipient in recipients)
                  {
                      try
                      {
                          await _emailService.SendEmailAsync(
                              recipient.Email,
                              campaign.Subject,
                              campaign.Body);

                          _logger.LogInformation("Sent email to: {RecipientEmail}", recipient.Email);
                      }
                      catch (Exception ex)
                      {
                          _logger.LogError(ex, "Failed to send email to: {RecipientEmail}", recipient.Email);
                      }
                  }

                  campaign.Status = CampaignStatus.Completed;
                  await _unitOfWork.SaveChangesAsync();

                  _logger.LogInformation("Completed Campaign ID: {CampaignId}", campaign.Id);
              }
          }
      */
    }
}
