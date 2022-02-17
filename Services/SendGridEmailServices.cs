using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Services
{
    public interface SendGridEmailServices
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridEmailSender : SendGridEmailServices
    {
        private readonly IConfiguration _config;

        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _config["SendGridApiKey"];
            var emailFrom = _config["EmailFrom"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(emailFrom, "noreply - SPMB");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            await client.SendEmailAsync(msg);
        }
    }
}
