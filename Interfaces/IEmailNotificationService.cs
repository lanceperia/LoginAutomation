namespace EmaptaLoginAutomation.Interfaces
{
    public interface IEmailNotificationService
    {
        void SendEmail(string subject, string message);
    }
}
