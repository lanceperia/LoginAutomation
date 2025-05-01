using EmaptaLoginAutomation.Interfaces;
using RestSharp;
using RestSharp.Authenticators;

namespace EmaptaLoginAutomation.Services
{
    public class MailgunEmailService(ILoggerService loggerService) : IEmailNotificationService
    {
        public void SendEmail(string subject, string message)
        {
            var apiPath = Environment.GetEnvironmentVariable("MAILGUN_ENDPOINT");
            var apiKey = Environment.GetEnvironmentVariable("MAILGUN_APIKEY");
            var recipient = Environment.GetEnvironmentVariable("MAILGUN_RECIPIENT");
            var sender = Environment.GetEnvironmentVariable("MAILGUN_SENDER");

            if (string.IsNullOrWhiteSpace(apiPath) ||
                string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrEmpty(recipient) ||
                string.IsNullOrWhiteSpace(sender))
            {
                loggerService.Information("Mailgun is not properly set up");

                return;
            }

            loggerService.Information($"Sending Email...");

            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };

            var client = new RestClient(options);
            var request = new RestRequest(apiPath, Method.Post)
            {
                AlwaysMultipartFormData = true
            };
            request.AddParameter("from", sender);
            request.AddParameter("to", recipient);
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);

            var response = client.ExecuteAsync(request).Result;

            loggerService.Information($"Status: {response.StatusCode}");
        }
    }
}
