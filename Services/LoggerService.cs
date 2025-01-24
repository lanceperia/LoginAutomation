﻿using EmaptaLoginAutomation.Interfaces;
using System.Reflection;

namespace EmaptaLoginAutomation.Services
{
    public class LoggerService : ILoggerService
    {
        public void Information(string message)
        {
            WriteLog(message, false);

        }

        public void Error(string message)
        {
            WriteLog(message, true);
        }

        private void WriteLog(string message, bool isError)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{path}/logs.txt";
            var time = DateTime.Now.ToString("T");
            var messageType = !isError ? "INF" : "ERR";

            try
            {
                // Write the text to the file
                File.AppendAllText(filePath, $"\n[{time} {messageType}]: {message}");

                Console.WriteLine(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
