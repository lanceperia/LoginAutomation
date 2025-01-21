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
            var dtrDescription = componentService
                .GetComponent("dtr-description", GetBy.Class);

            return dtrDescription != null && 
                dtrDescription.Text.Contains("Today is your rest day");
        }

        public bool IsShiftStarting()
        {
            return componentService
                .GetComponent(ClockInElementName, GetBy.Class, 5_000) is not null;
        }

        public bool IsShiftEnding()
        {
            return componentService
                .GetComponent(ClockOutElementName, GetBy.Id, 5_000) is not null;
        }

        public bool IsShiftCompleted()
        {
            return componentService
                .GetComponent("button-shiftended", GetBy.Class, 5_000) is not null;
        }

        // Private Method
        private bool ProcessAttendance(string componentName, GetBy getBy)
        {
            try
            {
                var component = componentService.GetComponent(componentName, getBy, timeOut: 10_000);
                if (component is not null)
                {
                    logger.Log($"Clicking {componentName}");

                    component.Click();

                    logger.Log($"Successfully clicked {componentName}");

                    return true;
                }

                logger.Log($"Component is null");

                return false;
            }
            catch (Exception e)
            {
                logger.Log($"ERROR: {e.Message}");

                return false;
            }
        }

    }
}
