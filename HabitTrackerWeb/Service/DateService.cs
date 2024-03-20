using HabitTrackerWeb.Controllers.Services;

namespace HabitTrackerWeb.Service
{
    public class DateService: IDateService
    {
        public DateOnly LastMonday()
        {
            DateTime today = DateTime.Today;
            DayOfWeek dayOfWeek = today.DayOfWeek;
            int daysFromMonday = ((int)dayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime lastMonday = today.AddDays(-daysFromMonday);
            return DateOnly.FromDateTime(lastMonday);   
        }
    }
}
