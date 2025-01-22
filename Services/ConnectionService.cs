using EmaptaLoginAutomation.Interfaces;

namespace EmaptaLoginAutomation.Services
{
    public class ConnectionService(ILoggerService logger) : IConnectionService
    {
        public bool HasInternetConnection()
        {
            var hasInternetConnection = false;
            var counter = 1;

            while (!hasInternetConnection && counter <= 4)
            {
                logger.Information($"Checking internet connection... ({counter})");

                try
                {
                    using var httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(5);
                    var response = httpClient.GetAsync("https://www.google.com").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        logger.Information($"Successfully connected to the internet...");
                        return true;
                    }
                }
                catch
                {
                    logger.Information($"No connection will retry...");
                }

                counter++;

                Thread.Sleep(30000);
            }

            logger.Error($"No Internet Connection...");
            return false;
        }
    }
}
