using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EmaptaLoginAutomation.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EmaptaLoginAutomation.Services
{
    public class SNSEmailService(ILoggerService loggerService, IConfiguration configuration) : IEmailNotificationService
    {
        public void SendEmail(string subject, string message)
        {
            var accessKeyId = configuration["AWS:AccessKeyId"];
            var secretAccessKey = configuration["AWS:SecretAccessKey"];
            var topicArn = configuration["AWS:SnsArn"];
            var client = new AmazonSimpleNotificationServiceClient(accessKeyId, secretAccessKey);

            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = message,
            };

            var response = client.PublishAsync(request).Result;

            loggerService.Log($"Status: {response.HttpStatusCode}");
        }
    }
}
