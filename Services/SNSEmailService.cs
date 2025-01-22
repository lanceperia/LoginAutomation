using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using EmaptaLoginAutomation.Interfaces;

namespace EmaptaLoginAutomation.Services
{
    public class SNSEmailService(ILoggerService loggerService) : IEmailNotificationService
    {
        public void SendEmail(string subject, string message)
        {
            var accessKeyId = Environment.GetEnvironmentVariable("PERSONAL_AWS_ACCESS_KEY_ID");
            var secretAccessKey = Environment.GetEnvironmentVariable("PERSONAL_AWS_SECRET_ACCESS_KEY");
            var topicArn = Environment.GetEnvironmentVariable("PERSONAL_AWS_SNS_ARN");

            if (string.IsNullOrWhiteSpace(accessKeyId) || 
                string.IsNullOrWhiteSpace(secretAccessKey) || 
                string.IsNullOrWhiteSpace(topicArn))
            {
                loggerService.Error("AWS SNS is not properly setup!");
                return;
            }

            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var client = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.APSoutheast1);
            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = message,
            };

            var response = client.PublishAsync(request).Result;

            loggerService.Information($"Status: {response.HttpStatusCode}");
        }
    }
}
