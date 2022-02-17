using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Services
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public MailJetOptions _mailJetOption;

        public MailJetEmailSender(IConfiguration config, MailJetOptions mailJetOption )
        {
            _config = config;
            _mailJetOption = mailJetOption;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            _mailJetOption = _config.GetSection("MailJet").Get<MailJetOptions>();

            MailjetClient client = new MailjetClient(_mailJetOption.ApiKey, _mailJetOption.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "amin@idwal.com.my"},
        {"Name", "amin"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "amin@idwal.com.my"
         }, {
          "Name",
          "amin"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       htmlMessage
      }, 
     }
             });
            await client.PostAsync(request);
            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            //    Console.WriteLine(response.GetData());
            //}
            //else
            //{
            //    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            //    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            //    Console.WriteLine(response.GetData());
            //    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            //}
        }
    }
}
