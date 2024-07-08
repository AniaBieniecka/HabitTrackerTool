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
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace HabitTrackerWeb.Controllers
{
    [Authorize]
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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitWeek> habitWeekList = _unitOfWork.HabitWeek.GetAll(u => u.WeekNumber == week && u.Year == year && u.habit.UserId == userId, includeProperties: "habitRealizations,habit").ToList();
            foreach(var habitWeek in habitWeekList)
            {
                habitWeek.habit.ViewSetting = _unitOfWork.ViewSetting.Get(u=>u.UserId == userId);
            }
            return View(habitWeekList);
        }

        public IActionResult HabitsCurrentWeek()
        {

            int weekCurrent = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateOnly mondayDate = _dateService.LastMonday();
            int year = mondayDate.Year;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitWeek> habitWeekList = _unitOfWork.HabitWeek.GetAll(u => u.WeekNumber == weekCurrent && u.Year == year && u.habit.UserId == userId, includeProperties: "habit,habitRealizations").ToList();

            int weekPrevious = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateOnly lastMondayDate = _dateService.LastMonday();
            DateOnly mondayDateFromPreviousWeek = lastMondayDate.AddDays(-7);
            int yearPreviousWeek = mondayDateFromPreviousWeek.Year;

            int howManyHabitsInPreviousWeek = _unitOfWork.HabitWeek.GetAll(u => u.WeekNumber == weekPrevious && u.Year == yearPreviousWeek && u.habit.UserId == userId).Count();

            var viewSettings = _unitOfWork.ViewSetting.Get(u=>u.UserId == userId);

            HabitsCurrentWeekVM habitsCurrentWeekVM = new HabitsCurrentWeekVM
            {
                habitWeekList = habitWeekList,
                howManyHabitsInPreviousWeek = howManyHabitsInPreviousWeek,
                score = _unitOfWork.Score.Get(u => u.UserId == userId),
                numberOfWeeks = HowManyWeeks(),
            };

            foreach (var habitWeek in habitsCurrentWeekVM.habitWeekList)
            {
                habitWeek.habit.ViewSetting = _unitOfWork.ViewSetting.Get(u => u.UserId == userId);
            }

            return View(habitsCurrentWeekVM);
        }

        [HttpGet]
        public IActionResult UpdateIfExecuted(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                var score = _unitOfWork.Score.Get(u => u.UserId == userId);
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

                var isGoalStatusUpdated = UpdateGoalStatus(item.HabitWeekId);
                var habitWeek = _unitOfWork.HabitWeek.Get(u => u.Id == item.HabitWeekId);

                if (isGoalStatusUpdated)
                {
                    if (habitWeek.IsWeeklyGoalAchieved == true)
                    {
                        scoreValue += 50;
                    }
                    else scoreValue -= 50;
                }

                score.ScoreValue = scoreValue;

                score.LevelId = SetLevel(score.LevelId, scoreValue);

                _unitOfWork.Score.Update(score);

                _unitOfWork.Save();

                return Json(new { ifExecuted = item.IfExecuted, scoreValue = score.ScoreValue, level = score.LevelId, weeklyGoalStatus = habitWeek.IsWeeklyGoalAchieved, isGoalStatusUpdated = isGoalStatusUpdated });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(404);
            }
        }
        public IActionResult Progress()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitRealization> habitRealizations = _unitOfWork.HabitRealization.GetAll(u=>u.habitWeek.habit.UserId == userId).ToList();
            int howManyHabitsInMonth;

            if (habitRealizations.Count != 0)
            {
                var endDate = habitRealizations.OrderBy(d => d.Date).LastOrDefault().Date;
                var startDate = HabitRealizationControllerHelper.StartDayOfChart(endDate);
                var startWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var endWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(endDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var startYear = startDate.Year;
                var endYear = endDate.Year;

                if (startYear == endYear)
                {
                    howManyHabitsInMonth = _unitOfWork.HabitWeek.GetAll
                        (u => u.WeekNumber >= startWeek && u.Year == startYear && u.habit.UserId == userId).GroupBy(u => u.HabitId).Count();

                }
                else
                    howManyHabitsInMonth = _unitOfWork.HabitWeek.GetAll
                        (u => (u.WeekNumber >= startWeek && u.Year == startYear && u.habit.UserId == userId) || (u.WeekNumber <= endWeek && u.Year == endYear && u.habit.UserId == userId))
                        .GroupBy(u => u.HabitId).Count();

                int chartsQuantity = (int)Math.Ceiling((double)howManyHabitsInMonth / 5);
                return View(chartsQuantity);
            }

            else return View();

        }
        public IActionResult GetProgressChartData()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var endDate = DateOnly.FromDateTime(DateTime.Now);
            var startDate = HabitRealizationControllerHelper.StartDayOfChart(endDate);
            var habitRealizations = _unitOfWork.HabitRealization.GetAll(u=> u.habitWeek.habit.UserId == userId).ToList().FindAll(u => u.Date >= startDate).OrderBy(x => x.Date);
            bool isDateFound = false;
            ProgressChartVM progressChartVM = new();


            if (habitRealizations.Count() != 0)
            {
                foreach (var habitRealization in habitRealizations)
                {
                    if (startDate == habitRealization.Date)
                    {
                        isDateFound = true;
                        break;
                    }
                }

                if (!isDateFound)
                {
                    startDate = habitRealizations.FirstOrDefault().Date;
                }

                List<HabitWeek> habitWeekList = _unitOfWork.HabitWeek.GetAll(u => u.habit.UserId == userId).ToList();
                List<ProgressChartData> progressChartData = new List<ProgressChartData>();

                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    float value = 0;

                    foreach (var habitWeek in habitWeekList)
                    {

                        var data = habitWeek.habitRealizations.FirstOrDefault(u => u.Date == date);
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

                progressChartVM.Series = chartDataList;
                progressChartVM.Categories = categoriesLineChart;

            }
            return Json(progressChartVM);

        }

        public IActionResult GetProgressChartDataForEachHabit(int chartNumber)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitRealization> allHabitRealizations = _unitOfWork.HabitRealization.GetAll(u => u.habitWeek.habit.UserId == userId).OrderBy(x => x.Date).ToList();
            List<ProgressChartData> progressChartData = new List<ProgressChartData>();
            List<ChartData> chartDataList = new();
            List<DateOnly> categoriesLineChart = new();

            if (allHabitRealizations.Count != 0)
            {
                var endDate = DateOnly.FromDateTime(DateTime.Now);
                var startDate = HabitRealizationControllerHelper.StartDayOfChart(endDate);
                var habitRealizations = allHabitRealizations.FindAll(u => u.Date >= startDate);
                bool isDateFound = false;

                foreach (var habitRealization in habitRealizations)
                {
                    if (startDate == habitRealization.Date)
                    {
                        isDateFound = true;
                        break;
                    }
                }

                if (!isDateFound)
                {
                    startDate = habitRealizations.FirstOrDefault().Date;
                }

                var startWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var endWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(endDate.ToDateTime(TimeOnly.MinValue), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var startYear = startDate.Year;
                var endYear = endDate.Year;
                List<Habit> habits;
                int habitsSkipped = chartNumber * 5;

                var allHabitWeeks = _unitOfWork.HabitWeek.GetAll(u => u.habit.UserId == userId, includeProperties: "habit,habitRealizations").ToList();
                var currentHabitWeekList = new List<HabitWeek>();

                if (startYear == endYear)
                {
                    currentHabitWeekList = allHabitWeeks.FindAll
                        (u => u.WeekNumber >= startWeek && u.Year == startYear)
                        .OrderByDescending(x => x.Id).ToList();
                }
                else
                    currentHabitWeekList = allHabitWeeks.FindAll
                        (u => (u.WeekNumber >= startWeek && u.Year == startYear) || (u.WeekNumber <= endWeek && u.Year == endYear))
                        .OrderByDescending(x => x.Id)
                        .Take(5).ToList();

                var groupedHabitWeekList = currentHabitWeekList.GroupBy(g => g.HabitId).Skip(habitsSkipped).Take(5).ToList();

                for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    categoriesLineChart.Add(date);
                }

                foreach (var habitWeekList in groupedHabitWeekList)
                {
                    progressChartData.Clear();

                    for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        float value = 0;
                        var habitRealization = new HabitRealization();

                        foreach (var habitWeek in habitWeekList)
                        {
                            habitRealization = habitWeek.habitRealizations.FirstOrDefault(u => u.Date == date);


                            if (habitRealization is not null)
                            {
                                if (habitRealization.IfExecuted == 1)
                                {
                                    value += 10;
                                }
                                else if (habitRealization.IfExecuted == 2)
                                {
                                    value += 5;
                                }
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
                        Name = habitWeekList.FirstOrDefault().habit.Name,
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

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitWeek> allHabitWeekList = _unitOfWork.HabitWeek.GetAll(u => u.habit.UserId == userId).ToList();
            List<int> weekList = new List<int>();

            foreach (var hab in allHabitWeekList)
            {
                weekList.Add(hab.WeekNumber);
            }

            var uniqueWeekList = weekList.Distinct().ToList();

            return uniqueWeekList.Count();
        }

        private bool UpdateGoalStatus(int habitWeekId)
        {
            var habitWeek = _unitOfWork.HabitWeek.Get(u => u.Id == habitWeekId);

            var habitsRealization = _unitOfWork.HabitRealization.GetAll(u => u.HabitWeekId == habitWeekId);
            int quantityOfExecuted = 0;
            bool wasUpdated = false;

            foreach (var obj in habitsRealization)
            {
                if (obj.IfExecuted == 1)
                    quantityOfExecuted++;
            }

            var isFulfilled = false;
            if (quantityOfExecuted >= habitWeek.WeeklyGoal)
            {
                isFulfilled = true;
            }
            else isFulfilled = false;

            if (habitWeek.IsWeeklyGoalAchieved != isFulfilled)
            {
                habitWeek.IsWeeklyGoalAchieved = isFulfilled;
                _unitOfWork.HabitWeek.Update(habitWeek);
                _unitOfWork.Save();
                wasUpdated = true;
            }

            return wasUpdated;
        }


    }
}
