using Jumia_Api.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Jumia_Api.Application.Services
{
    public class ConfirmationEmailService : IConfirmationEmailService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ConfirmationEmailService> _logger;

        public ConfirmationEmailService(IEmailService emailService, ILogger<ConfirmationEmailService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

       

        public void SendConfirmationEmailAsync(string email, string token, string status)
        {
            // Fire-and-forget using ThreadPool
            ThreadPool.QueueUserWorkItem(async _ =>
            {
                string message = string.Empty;
                string htmlMessage = string.Empty;
                if (status == "otpcode")
                {
                     message = $"Your OTP code is: {token}. Please use this code to complete your registration.";
                    htmlMessage = $"<p>Your OTP code is: <strong>{token}</strong>. Please use this code to complete your registration.</p>";
                }
                if(status == "confirmation link")
                {
                    message = $"https://yourapp.com/confirm?token={token}";
                    htmlMessage = $"<p>Click the link to confirm your account: <a href='https://yourapp.com/confirm?token={token}'>Confirm Account</a></p>";


                }
               

                try
                {
                    if(!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(htmlMessage))
                    {
                        
                    await _emailService.SendEmailAsync(email, "Confirm your account", htmlMessage);
                    }
                    else
                    {
                        throw new ArgumentException("Message or HTML message cannot be empty");
                    }
                   
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