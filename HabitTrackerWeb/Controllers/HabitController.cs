using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;

namespace HabitTrackerWeb.Controllers
{
    public class HabitController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public HabitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            DateTime now = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(now);
            // to the loopDate at first we assign Monday date;
            DateOnly loopDate = today.AddDays(-(int)today.DayOfWeek); 

            if (ModelState.IsValid)
            {
                habit.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                habit.Year = now.Year;
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
    }
}
