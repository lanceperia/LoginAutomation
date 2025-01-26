using EmaptaLoginAutomation.Enums;
using EmaptaLoginAutomation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmaptaLoginAutomation
{
    public class App(ILoggerService logger,
        IComponentService componentService,
        IConnectionService connectionService,
        IAttendanceService attendanceService,
        IEmailNotificationService emailService,
        ChromeDriver driver)
    {
        public void Run()
        {
            // Check if there's internet connection
            if (!connectionService.HasInternetConnection())
            {
                return;
            }

            // Go to Emapta Website
            driver.Navigate().GoToUrl("https://portal.empowerteams.io/login");

            // Login
            var username = $"{Environment.GetEnvironmentVariable("PRIMARY_USERNAME")}";
            var password = $"{Environment.GetEnvironmentVariable("PRIMARY_PASSWORD")}";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                logger.Information("Invalid login credentials");

                return;
            }

            if (!HasLoggedIn(username, password))
            {
                return;
            }

            // Check Restday
            if (attendanceService.IsRestDay() || 
                attendanceService.IsOnLeave() ||
                attendanceService.IsHoliday())
            {
                logger.Information("TODAY IS YOUR REST DAY!");
                emailService.SendEmail("Rest Day", "Today is your rest day. Don't bother working");

                return;
            }

            // Check Shift
            if (attendanceService.IsShiftCompleted())
            {
                logger.Information($"Your shift has already completed");
                emailService.SendEmail("Shift Completed", $"Your shift has already completed");

                return;
            }

            if (attendanceService.IsShiftStarting())
            {
                ProcessClockIn();
                return;
            }

            if (attendanceService.IsShiftEnding())
            {
                ProcessClockOut();
                return;
            }

            logger.Information($"Shift not yet starting");
            emailService.SendEmail("Shift not starting", $"Your shift is not yet starting");
        }

        // Private Methods
        private void ProcessClockIn()
        {
            var attendance = attendanceService.ClockIn();

            if (attendance.HasClockedIn)
            {
                logger.Information("Clocked In Successfully!");
                emailService.SendEmail("Clock In", $"Clocked in at {DateTime.Now:t}");
                return;
            }

            emailService.SendEmail("Failed", $"Clock In failed");
        }
        private void ProcessClockOut()
        {
            var attendance = attendanceService.ClockOut();

            if (attendance.HasClockedOut)
            {
                logger.Information("Clocked Out Successfully!");
                emailService.SendEmail("Clock Out", $"Clocked out at {DateTime.Now:t}");
                return;
            }

            emailService.SendEmail("Failed", $"Clock Out failed");
        }
        private bool HasLoggedIn(string userName, string password)
        {
            var loginButtonName = "//button[text()='Login as Employee']";
            var loginButton = componentService.GetComponent(loginButtonName, GetBy.XPath, 10_000);
            if (loginButton is null)
            {
                emailService.SendEmail("Failed", "Login didn't load :(");

                return false;
            }

            loginButton.Click();

            var flutter = componentService.GetComponent("flutter-view", GetBy.Tag, 60_000);
            if (flutter is null)
            {
                emailService.SendEmail("Failed", "UserName page didn't load :(");

                return false;
            }

            flutter.SendKeys(Keys.Tab);
            flutter.SendKeys(userName);
            flutter.SendKeys(Keys.Tab);
            flutter.SendKeys(Keys.Enter);

            logger.Information("Username entered");
            Thread.Sleep(2_000);

            flutter.SendKeys(Keys.Tab);
            flutter.SendKeys(password);
            flutter.SendKeys(Keys.Tab);
            flutter.SendKeys(Keys.Enter);

            logger.Information("Logging in...");
            Thread.Sleep(10_000);

            loginButton = componentService.GetComponent(loginButtonName, GetBy.XPath, 30_000);
            if (loginButton != null)
            {
                loginButton.Click();
                Thread.Sleep(5_000);
            }

            return true;
        }
    }
}
