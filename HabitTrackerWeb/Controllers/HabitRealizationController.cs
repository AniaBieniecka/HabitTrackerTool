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

                var isCommitmentStatusUpdated = UpdateCommitmentStatus(item.HabitId);
                var habit = _unitOfWork.Habit.Get(u => u.Id == item.HabitId);

                if (isCommitmentStatusUpdated)
                {
                    if(habit.IsWeeklyCommitmentFulfilled == true)
                    {
                    scoreValue += 50;
                    }
                    else scoreValue -= 50;
                }

                score.ScoreValue = scoreValue;

                score.LevelId = SetLevel(score.LevelId, scoreValue);

                _unitOfWork.Score.Update(score);

                _unitOfWork.Save();

                return Json(new { ifExecuted = item.IfExecuted, scoreValue = score.ScoreValue, level = score.LevelId, commitmentStatus = habit.IsWeeklyCommitmentFulfilled, isCommitmentUpdated = isCommitmentStatusUpdated});
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(404);
            }
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

        private bool UpdateCommitmentStatus(int habitId)
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
            if (quantityOfExecuted >= habit.QuantityPerWeek)
            {
                isFulfilled = true;
            }
            else isFulfilled = false;

            if (habit.IsWeeklyCommitmentFulfilled != isFulfilled)
            {
                habit.IsWeeklyCommitmentFulfilled = isFulfilled;
                _unitOfWork.Habit.Update(habit);
                _unitOfWork.Save();
                wasUpdated = true;
            }

            return wasUpdated;
        }
    }
}
