using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EmaptaLoginAutomation.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EmaptaLoginAutomation.Services
{
    public class SNSEmailService(ILoggerService loggerService) : IEmailNotificationService
    {
        public void SendEmail(string subject, string message)
        {
            var accessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var topicArn = Environment.GetEnvironmentVariable("AWS_SNS_ARN");
            var client = new AmazonSimpleNotificationServiceClient(accessKeyId, secretAccessKey);
            
            loggerService.Log($"Variables: {topicArn}");

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
