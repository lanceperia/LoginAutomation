using EmaptaLoginAutomation.Interfaces;
using EmaptaLoginAutomation.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EmaptaLoginAutomation.Services
{
    public class MailerSendEmailService(ILoggerService loggerService, IConfiguration configuration) : IEmailNotificationService
    {
        public void SendEmail(string subject, string message)
        {

            return;

            // TRIAL PLAN HAS BEEN CONSUMED
            var token = configuration["MailerSend:ApiKey"];
            var templateId = configuration["MailerSend:Template"];
            var sender = configuration["MailerSend:Sender"];

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(sender))
            {
                loggerService.Error("(MailerSendEmailService): No Token/Sender found");
            }

            loggerService.Information("Sending email...");

            var recipients = configuration.GetSection("MailerSend:Recipients").GetChildren().Select(r => r.Value).ToList();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email");

            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var body = new EmailModel()
            {
                From = new RecipientModel()
                {
                    Email = sender,
                    Name = "DTR Automation"
                },
                To = recipients.Select(r => new RecipientModel() { Email = r }).ToList(),
                TemplateId = templateId,
                Subject = subject,
                Personalization = recipients.Select(r => new Personalization()
                {
                    Email = r,
                    Data = new Data()
                    {
                        Body = message
                    }
                }).ToList()
            };

            var serializedBody = JsonSerializer.Serialize(body);
            request.Content = new StringContent(serializedBody, null, "application/json");

            var response = client.Send(request);
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                loggerService.Information("Email sent");
                return;
            }

            loggerService.Error($"(MailerSendEmailService): Email was not sent {response.StatusCode} ({response.ReasonPhrase}) -- {result}");
        }
    }
}
