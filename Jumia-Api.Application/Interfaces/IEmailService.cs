using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    
    
        public interface IEmailService
        {
            //Task SendSmtpAsync(string toEmail, string subject, string htmlBody);
            public Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        }
    
}
