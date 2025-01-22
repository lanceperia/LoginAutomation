using Amazon;
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
            var client = new AmazonSimpleNotificationServiceClient(accessKeyId, secretAccessKey,RegionEndpoint.APSoutheast1);
            
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
