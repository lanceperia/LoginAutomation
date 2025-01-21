using EmaptaLoginAutomation.Interfaces;
using System.Reflection;

namespace EmaptaLoginAutomation.Services
{
    public class LoggerService : ILoggerService
    {
        public void Log(string message)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{path}/logs.txt";
            var textToWrite = $"\n[{DateTime.Now.ToString("T")}]: {message}";

            try
            {
                // Write the text to the file
                File.AppendAllText(filePath, textToWrite);

                Console.WriteLine(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
