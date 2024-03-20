using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTrackerWeb.Controllers.Services;
using HabitTrackerWeb.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;

namespace HabitTrackerWeb.Controllers
{
    public class HabitController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateService _dateService;
        public HabitController(IUnitOfWork unitOfWork, IDateService dateService)
        {
            _unitOfWork = unitOfWork;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            List<Habit> objHabitList = _unitOfWork.Habit.GetAll().ToList();
            return View(objHabitList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Habit habit)
        {
            DateOnly mondayDate = _dateService.LastMonday();

            // to the loopDate at first we assign Monday date;
            DateOnly loopDate = mondayDate;

            if (ModelState.IsValid)
            {
                habit.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                habit.Year = loopDate.Year;
                _unitOfWork.Habit.Add(habit);
                _unitOfWork.Save();

                for (int i = 0; i < 7; i++)
                {
                    HabitRealization habitDay = new HabitRealization();
                    habitDay.Date = loopDate;
                    loopDate = loopDate.AddDays(1);
                    habitDay.HabitId = habit.Id;
                    _unitOfWork.HabitRealization.Add(habitDay);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Habit created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult ShowWeeks()
        {
            List<Habit> habits = _unitOfWork.Habit.GetAll(includeProperties: "habitRealizations").ToList();

            List<WeekYear> weekYearList = new List<WeekYear>();

            foreach (Habit hab in habits)
            {
                var weekYear = new WeekYear { Week = hab.WeekNumber, Year = hab.Year };
                weekYearList.Add(weekYear);
            }

            var uniqueWeekYearList = weekYearList.Distinct().ToList();

            return View(uniqueWeekYearList);
        }
        public IActionResult ChooseHabits()
        {

            var habitSuggestion = new HabitSuggestion();
            var habitSuggestionToDisplay = new List<string>();
            var indexList = new List<int>();

            Random rnd = new Random();
            while (indexList.Count < 3)
            {
                int index = rnd.Next(habitSuggestion.HabitSuggestions.Count - 1);

                if (!indexList.Contains(index))
                {
                    indexList.Add(index);
                    habitSuggestionToDisplay.Add(habitSuggestion.HabitSuggestions[index]);
                }
            }

            int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) - 1;
            int year = DateTime.Now.Year;

            ChooseHabitsVM chooseHabitsVM = new ChooseHabitsVM
            {
                habits = _unitOfWork.Habit.GetAll(u => u.WeekNumber == week && u.Year == year, includeProperties: "habitRealizations").ToList(),
                habitSuggestions = habitSuggestionToDisplay
            };

            return View(chooseHabitsVM);
        }

        [HttpPost]
        public IActionResult ChooseHabits(string[] habitNames, int[] quantityPerWeek)
        {
            DateOnly mondayDate = _dateService.LastMonday();
            DateOnly loopDate = mondayDate;
            int index = 0;

            foreach (var habitName in habitNames)
            {
                Habit habit = new Habit();
                habit.Name = habitName;
                habit.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                habit.QuantityPerWeek = quantityPerWeek[index];
                index++;
                habit.Year = loopDate.Year;
                _unitOfWork.Habit.Add(habit);
                _unitOfWork.Save();

                for (int i = 0; i < 7; i++)
                {
                    HabitRealization habitDay = new HabitRealization();
                    habitDay.Date = loopDate;
                    loopDate = loopDate.AddDays(1);
                    habitDay.HabitId = habit.Id;
                    _unitOfWork.HabitRealization.Add(habitDay);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }

        public IActionResult CreateHabitsFromPreviousWeek()
        {
            int weekCurrent = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int weekPrevious = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateOnly lastMondayDate = _dateService.LastMonday();
            DateOnly mondayDateFromPreviousWeek = lastMondayDate.AddDays(-7);
            int year = mondayDateFromPreviousWeek.Year;
            DateOnly loopDate;

            List<Habit> habitsPreviousWeek = _unitOfWork.Habit.GetAll(u => u.WeekNumber == weekPrevious && u.Year == year, includeProperties: "habitRealizations").ToList();

            if (habitsPreviousWeek.Count > 0)
            {
                foreach (var habitPreviousWeek in habitsPreviousWeek)
                {
                    Habit habit = new Habit();
                    habit.Name = habitPreviousWeek.Name;
                    habit.QuantityPerWeek = habitPreviousWeek.QuantityPerWeek;
                    habit.WeekNumber = weekCurrent;
                    loopDate = lastMondayDate;
                    habit.Year = loopDate.Year;
                    _unitOfWork.Habit.Add(habit);
                    _unitOfWork.Save();

                    for (int i = 0; i < 7; i++)
                    {
                        HabitRealization habitDay = new HabitRealization();
                        habitDay.Date = loopDate;
                        loopDate = loopDate.AddDays(1);
                        habitDay.HabitId = habit.Id;
                        _unitOfWork.HabitRealization.Add(habitDay);
                        _unitOfWork.Save();
                    }
                }
                return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
            }
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }
    }
}

