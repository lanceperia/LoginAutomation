using EmaptaLoginAutomation.Enums;
using EmaptaLoginAutomation.Interfaces;
using EmaptaLoginAutomation.Models;

namespace EmaptaLoginAutomation.Services
{
    public class AttendanceService(IComponentService componentService,
        ILoggerService logger) : IAttendanceService
    {

        private readonly string ClockInElementName = "GA-clockin-button-topbar";
        private readonly string ClockOutElementName = "button-clockout";

        public AttendanceModel ClockIn()
        {
            var hasClockedIn = ProcessAttendance(ClockInElementName, GetBy.Id);

            return new AttendanceModel()
            {
                HasClockedIn = hasClockedIn,
            };
        }

        public AttendanceModel ClockOut()
        {
            var hasClockedOut = ProcessAttendance(ClockOutElementName, GetBy.Class);

            return new AttendanceModel()
            {
                HasClockedOut = hasClockedOut
            };
        }

        public bool IsRestDay()
        {
            return !IsWorkingDay("Today is your rest day");
        }

        public bool IsHoliday()
        {
            return !IsWorkingDay("Today is holiday");
        }

        public bool IsOnLeave()
        {
            return !IsWorkingDay("You are on leave today");
        }

        public bool IsShiftStarting()
        {
            return componentService
                .GetComponent(ClockInElementName, GetBy.Id, 5_000) is not null;
        }

        public bool IsShiftEnding()
        {
            return componentService
                .GetComponent(ClockOutElementName, GetBy.Class, 5_000) is not null;
        }

        public bool IsShiftCompleted()
        {
            return componentService
                .GetComponent("button-shiftended", GetBy.Class, 5_000) is not null;
        }


        // Private Method
        private bool IsWorkingDay(string message)
        {
            var dtrDescription = componentService
                           .GetComponent("dtr-description", GetBy.Class);

            logger.Information($"Finding {message}");

            return dtrDescription != null &&
                !dtrDescription.Text.Contains(message);
        }
        private bool ProcessAttendance(string componentName, GetBy getBy)
        {
            try
            {
                var component = componentService.GetComponent(componentName, getBy, timeOut: 10_000);
                if (component is not null)
                {
                    logger.Information($"Clicking {componentName}");

                    component.Click();

                    logger.Information($"Successfully clicked {componentName}");

                    return true;
                }

                logger.Information($"Component is null");

                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);

                return false;
            }
        }

    }
}
