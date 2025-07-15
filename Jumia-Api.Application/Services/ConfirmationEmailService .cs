using Jumia_Api.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class ConfirmationEmailService : IConfirmationEmailService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public ConfirmationEmailService(IEmailService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

       

        public void SendConfirmationEmailAsync(string email, string token)
        {
            // Fire-and-forget using ThreadPool
            ThreadPool.QueueUserWorkItem(async _ =>
            {
                var confirmationLink = $"https://yourapp.com/confirm?token={token}";
                var htmlMessage = $"<p>Click <a href='{confirmationLink}'>here</a> to confirm your email.</p>";

                try
                {
                    await _emailService.SendEmailAsync(email, "Confirm your account", htmlMessage);
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError("email have not been sent ");
                }
            });
        }
    }
}