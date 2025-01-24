using EmaptaLoginAutomation.Models;

namespace EmaptaLoginAutomation.Interfaces
{
    public interface IAttendanceService
    {
        AttendanceModel ClockIn();
        AttendanceModel ClockOut();
        bool IsShiftStarting();
        bool IsShiftEnding();
        bool IsShiftCompleted();
        bool IsRestDay();
        bool IsOnLeave();
    }
}
