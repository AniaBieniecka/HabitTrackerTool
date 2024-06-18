using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Repositry;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using HabitTrackerWeb.Controllers.Services;
using HabitTracker.Models.ViewModels;
using HabitTracker.Models.ScoringModels;
using static System.Formats.Asn1.AsnWriter;
using System.Linq;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace HabitTrackerWeb.Controllers
{
    public class HabitRealizationController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateService _dateService;

        public HabitRealizationController(IUnitOfWork unitOfWork, IDateService dateService)
        {
            _unitOfWork = unitOfWork;
            _dateService = dateService;
        }

        public IActionResult HabitsWeekly(int? week, int? year)
        {
            List<Habit> habits = _unitOfWork.Habit.GetAll(u => u.WeekNumber == week && u.Year == year, includeProperties: "habitRealizations").ToList();

            foreach (Habit hab in habits)
            {
                hab.ViewSetting = _unitOfWork.ViewSetting.Get(u => u.Id == 1);
            }

            return View(habits);
        }

        public IActionResult HabitsCurrentWeek()
        {

            int weekCurrent = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateOnly mondayDate = _dateService.LastMonday();
            int year = mondayDate.Year;

            List<Habit> habits = _unitOfWork.Habit.GetAll(u => u.WeekNumber == weekCurrent && u.Year == year, includeProperties: "habitRealizations").ToList();

            HabitsCurrentWeekVM habitsCurrentWeekVM = new HabitsCurrentWeekVM
            {
                habits = habits,
                habitsHasAnyData = _unitOfWork.Habit.HasAnyData(),
                score = _unitOfWork.Score.Get(u => u.Id == 1),
                numberOfWeeks = HowManyWeeks(),
            };

            foreach (Habit hab in habits)
            {
                hab.ViewSetting = _unitOfWork.ViewSetting.Get(u => u.Id == 1);
            }

            return View(habitsCurrentWeekVM);
        }

        [HttpGet]
        public IActionResult UpdateIfExecuted(int id)
        {
            try
            {
                var score = _unitOfWork.Score.Get(u => u.Id == 1);
                var scoreValue = score.ScoreValue;
                var item = _unitOfWork.HabitRealization.Get(u => u.Id == id);

                if (item == null)
                {
                    return NotFound();
                }

                if (item.IfExecuted == 0)
                {
                    scoreValue += 10;
                    item.IfExecuted = 1;

                }
                else if (item.IfExecuted == 1)
                {
                    scoreValue -= 5;
                    item.IfExecuted = 2;
                }
                else
                {
                    item.IfExecuted = 0;
                    scoreValue -= 5;
                }

                _unitOfWork.HabitRealization.Update(item);
                _unitOfWork.Save();

                var isGoalStatusUpdated = UpdateGoalStatus(item.HabitId);
                var habit = _unitOfWork.Habit.Get(u => u.Id == item.HabitId);

                if (isGoalStatusUpdated)
                {
                    if (habit.IsWeeklyGoalAchieved == true)
                    {
                        scoreValue += 50;
                    }
                    else scoreValue -= 50;
                }

                score.ScoreValue = scoreValue;

                score.LevelId = SetLevel(score.LevelId, scoreValue);

                _unitOfWork.Score.Update(score);

                _unitOfWork.Save();

                return Json(new { ifExecuted = item.IfExecuted, scoreValue = score.ScoreValue, level = score.LevelId, weeklyGoalStatus = habit.IsWeeklyGoalAchieved, isGoalStatusUpdated = isGoalStatusUpdated });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(404);
            }
        }
        public IActionResult Progress()
        {
            return View();
        }
        public IActionResult GetProgressChartData()
        {
            List<Habit> habits = _unitOfWork.Habit.GetAll(includeProperties: "habitRealizations").ToList();
            List<HabitRealization> habitRealizations = _unitOfWork.HabitRealization.GetAll().ToList();
            List<ProgressChartData> progressChartData = new List<ProgressChartData>();

            if (habitRealizations.Count != 0)
            {
                var endDate = habitRealizations.OrderBy(d => d.Date).LastOrDefault().Date;
                DateTime lastDayOfPreviousMonth = new DateTime(endDate.Year, endDate.Month, 1).AddDays(-1);
                int daysOnTheChart = DateTime.DaysInMonth(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month);
                var startDate = endDate.AddDays(-daysOnTheChart);

                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    float value = 0;

                    foreach (var habit in habits)
                    {

                        var data = habit.habitRealizations.FirstOrDefault(u => u.Date == date);
                        if (data is not null)
                        {
                            if (data.IfExecuted == 1)
                            {
                                value += 10;
                            }
                            else if (data.IfExecuted == 2)
                            {
                                value += 5;
                            }
                        }

                    }
                    var newDataAllHabits = new ProgressChartData
                    {
                        Date = date,
                        Value = value
                    };
                    progressChartData.Add(newDataAllHabits);

                }

                var valuesLineChart = progressChartData.Select(u => u.Value).ToArray();
                var categoriesLineChart = progressChartData.Select(u => u.Date).ToArray();

                List<ChartData> chartDataList = new()
                {
                    new ChartData
                    {
                        Name ="All habits",
                        Data = valuesLineChart
                    },
                };

                ProgressChartVM progressChartVM = new()
                {
                    Series = chartDataList,
                    Categories = categoriesLineChart,
                };

                return Json(progressChartVM);

            }

            else

                return RedirectToAction("HabitsCurrentWeek");
        }

        public IActionResult GetProgressChartDataForEachHabit()
        {
            List<HabitRealization> habitRealizations = _unitOfWork.HabitRealization.GetAll().ToList();
            List<ProgressChartData> progressChartData = new List<ProgressChartData>();
            List<ChartData> chartDataList = new();
            List<DateOnly> categoriesLineChart = new();

            if (habitRealizations.Count != 0)
            {
                var endDate = habitRealizations.OrderBy(d => d.Date).LastOrDefault().Date;
                DateTime lastDayOfPreviousMonth = new DateTime(endDate.Year, endDate.Month, 1).AddDays(-1);
                int daysOnTheChart = DateTime.DaysInMonth(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month);
                var startDate = endDate.AddDays(-daysOnTheChart);

                var startWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var endWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(endDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var startYear = startDate.Year;
                var endYear = endDate.Year;
                List<Habit> habits;

                if (startYear == endYear)
                {
                    habits = _unitOfWork.Habit.GetAll(u => u.WeekNumber >= startWeek && u.Year == startYear, includeProperties: "habitRealizations").OrderByDescending(x=>x.Id).Take(5).ToList();

                }
                else
                    habits = _unitOfWork.Habit.GetAll(u => (u.WeekNumber >= startWeek && u.Year == startYear) || (u.WeekNumber <= endWeek && u.Year == endYear), includeProperties: "habitRealizations").OrderByDescending(x => x.Id).Take(5).ToList();

                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    categoriesLineChart.Add(date);
                }
                foreach (var habit in habits)
                {
                    progressChartData.Clear();

                    for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        float value = 0;
                        var data = habit.habitRealizations.FirstOrDefault(u => u.Date == date);
                        if (data is not null)
                        {
                            if (data.IfExecuted == 1)
                            {
                                value += 10;
                            }
                            else if (data.IfExecuted == 2)
                            {
                                value += 5;
                            }

                        }

                        var newData = new ProgressChartData
                        {
                            Date = date,
                            Value = value
                        };
                        progressChartData.Add(newData);
                    }

                    var valuesLineChart = progressChartData.Select(u => u.Value).ToArray();
                    ChartData chartData = new()
                    {
                        Name = habit.Name,
                        Data = valuesLineChart
                    };

                    chartDataList.Add(chartData);

                }
                var categories = categoriesLineChart.ToArray();

                ProgressChartVM progressChartVM = new()
                {
                    Series = chartDataList,
                    Categories = categories,
                };

                return Json(progressChartVM);

            }

            else

                return RedirectToAction("HabitsCurrentWeek");
        }

        //supporting methods
        private int SetLevel(int levelId, int scoreValue)
        {
            var level = new Level(levelId);
            int minimumNextLevelScore = 0;
            minimumNextLevelScore = levelId * 500;

            if (scoreValue >= level.MinimumScoreNextLevel)
            {
                levelId++;
            }

            if (scoreValue < level.MinimumScoreNextLevel - 500 && levelId != 1)
            {
                levelId--;
            }

            return levelId;
        }

        private int HowManyWeeks()
        {

            //counting how many weeks are in DB

            List<Habit> allHabits = _unitOfWork.Habit.GetAll().ToList();
            List<int> weekList = new List<int>();

            foreach (Habit hab in allHabits)
            {
                weekList.Add(hab.WeekNumber);
            }

            var uniqueWeekList = weekList.Distinct().ToList();

            return uniqueWeekList.Count();
        }

        private bool UpdateGoalStatus(int habitId)
        {
            var habit = _unitOfWork.Habit.Get(u => u.Id == habitId);
            var habitsRealization = _unitOfWork.HabitRealization.GetAll(u => u.HabitId == habitId);
            int quantityOfExecuted = 0;
            bool wasUpdated = false;

            foreach (var obj in habitsRealization)
            {
                if (obj.IfExecuted == 1)
                    quantityOfExecuted++;
            }

            var isFulfilled = false;
            if (quantityOfExecuted >= habit.WeeklyGoal)
            {
                isFulfilled = true;
            }
            else isFulfilled = false;

            if (habit.IsWeeklyGoalAchieved != isFulfilled)
            {
                habit.IsWeeklyGoalAchieved = isFulfilled;
                _unitOfWork.Habit.Update(habit);
                _unitOfWork.Save();
                wasUpdated = true;
            }

            return wasUpdated;
        }


    }
}
