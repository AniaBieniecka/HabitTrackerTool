namespace HabitTrackerWeb.Controllers
{
    public static class HabitRealizationControllerHelper
    {
        public static DateOnly StartDayOfChart(DateOnly endDate)
        {
            DateTime lastDayOfPreviousMonth = new DateTime(endDate.Year, endDate.Month, 1).AddDays(-1);
            int daysOnTheChart = DateTime.DaysInMonth(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month);
            var  startDate = endDate.AddDays(-daysOnTheChart);
            return startDate;
        }
    }
}
