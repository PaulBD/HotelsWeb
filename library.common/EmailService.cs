using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;

namespace library.common
{
    public class EmailService : IEmailService
    {
        protected string _apiKey;
        protected string _url;
        protected string _fromEmail;
        protected string _password;
        protected EncryptionService _encryptionService;

        public EmailService()
        {
            _apiKey = ConfigurationManager.AppSettings["mandrill.key"];
            _password = ConfigurationManager.AppSettings["encryption.password"];
            _url = ConfigurationManager.AppSettings["Url"];
            _fromEmail = "help@triperoo.co.uk";
            _encryptionService = new EncryptionService();
        }

        public void SendWelcomeEmail(string toEmail, string name, string town)
        {
            if (town.Contains(','))
            {
                town = town.Split(',')[0];
            }

            //AddToList(toEmail, name, town);

            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "FNAME", Content = name},
                new TemplateContent { Name = "TOWN", Content = town }
            };

            var status = SendEmail(toEmail, name, "Welcome to Triperoo", "welcome", templateContent);
        }

        public void SendForgotPasswordReminder(string toEmail, string name, string guid)
        {
            var link = _url + "/" + System.Net.WebUtility.UrlEncode(_encryptionService.EncryptText(guid, _password)) + "/forgot-password";

            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "FNAME", Content = name},
                new TemplateContent { Name = "LINK", Content = link }
            };

            var status = SendEmail(toEmail, name, "Forgot Password", "forgot-password", templateContent);
        }

        public void SendForgotPasswordConfirmation(string toEmail, string name)
        {
            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "FNAME", Content = name}
            };

            var status = SendEmail(toEmail, name, "Password Confirmation", "forgot-password-confirmation", templateContent);
        }

        private async Task<EmailResultStatus> SendEmail(string toEmail, string name, string subject, string templateName, List<TemplateContent> templateContent)
        {
            var api = new MandrillApi(_apiKey);
            List<EmailResult> result = await api.SendMessageTemplate(new Mandrill.Requests.Messages.SendMessageTemplateRequest(

                new EmailMessage
                {
                    To = new List<EmailAddress> { new EmailAddress { Email = toEmail, Name = name } },
                    FromEmail = _fromEmail,
                    FromName = "Triperoo",
                    Subject = subject
                },
                templateName,
                templateContent
            ));

            return result.First().Status;
        }

        public async void AddToList(string emailAddress, string name, string town)
        {
            var listId = "Triperoo";
            var member = new MailChimp.Api.Net.Domain.Lists.MCMember();
            member.email_address = emailAddress;
            member.merge_fields.Add("FNAME", name);
            member.merge_fields.Add("TOWN", town);

            MailChimp.Api.Net.Services.Lists.MailChimpList list = new MailChimp.Api.Net.Services.Lists.MailChimpList();
            await list.AddMember(member, listId);
        }
    }
}
