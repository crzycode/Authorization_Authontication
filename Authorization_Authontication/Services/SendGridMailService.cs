using Authorization_Authontication.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services
{
    public class SendGridMailService : IMailServices
    {
        public SendGridMailService(IConfiguration config)
        {
            Config = config;
        }

        public IConfiguration Config { get; }

        public async Task Sendemailasync(string toemail, string content)
        {
            try
            {
                var apiKey = Config["SendGridApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(Config["senderemail"], "this is mail singh");
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress(toemail);
                var plainTextContent = "and easy to do anywhere, even with C#";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, content);
                var respose = await client.SendEmailAsync(msg);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
