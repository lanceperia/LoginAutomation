using EmaptaLoginAutomation.Interfaces;
using EmaptaLoginAutomation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;
using System.Reflection;

namespace EmaptaLoginAutomation
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create a service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILoggerService>();

            try
            {
                // Resolve and run the application
                var app = serviceProvider.GetService<App>();
                app!.Run();
            }
            catch (Exception e)
            {
                logger.Error($"EXCEPTION: {e.Message} -- {e.StackTrace}");
            }

            // Close browser
            Dispose(serviceProvider);
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<App>();
            services.AddSingleton<IConfiguration>(provider =>
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return new ConfigurationBuilder()
                    .SetBasePath(path!)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            });
            services.AddSingleton(provider =>
            {
                var options = new ChromeOptions();

                options.AddArguments(@"user-data-dir=C:\Users\LancePeria\AppData\Local\Google\Chrome\User Data");
                options.AddArguments(@"profile-directory=Default");


                return new ChromeDriver(options);
            });

            // Register other services
            services.AddSingleton<IEmailNotificationService, SNSEmailService>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IComponentService, ComponentService>();
            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IAttendanceService, AttendanceService>();
        }

        private static void Dispose(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILoggerService>();
            var driver = provider.GetRequiredService<ChromeDriver>();

            logger.Information("Closing browser...");
            driver.Quit();
        }

    }
}