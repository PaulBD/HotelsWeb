using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;

namespace library.common
{
    public class Email : IEmail
    {
        protected string _apiKey;
        protected string _fromEmail;

        public Email()
        {
            _apiKey = ConfigurationManager.AppSettings["mandrill.key"];
            _fromEmail = "help@triperoo.co.uk";
        }

        public void SendWelcomeEmail(string toEmail, string name, string town)
        {
            //AddToList(toEmail, name, town);

            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "fullname", Content = name},
                new TemplateContent { Name = "town", Content = town }
            };

            var status = SendEmail(toEmail, name, "Welcome to Triperoo", "welcome", templateContent);
        }

        public void SendForgotPasswordReminder(string toEmail, string name, string link)
        {
            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "fullname", Content = name},
                new TemplateContent { Name = "link", Content = link }
            };

            var status = SendEmail(toEmail, name, "Forgot Password", "forgotPasswordReminderEmail", templateContent);
        }

        public void SendForgotPasswordConfirmation(string toEmail, string name)
        {
            var templateContent = new List<TemplateContent>
            {
                new TemplateContent { Name = "fullname", Content = name}
            };

            var status = SendEmail(toEmail, name, "Password Confirmation", "forgotPasswordConfirmationEmail", templateContent);
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
