using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create(Habit obj)
        {
            DateTime now = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(now);
            DateOnly mondayDate = today.AddDays(-(int)today.DayOfWeek); // - Monday date
            DateOnly date;

            int GetWeekNumber(DateTime date)
            {
                CultureInfo myCI = new CultureInfo("pl-PL");
                Calendar myCal = myCI.Calendar;
                CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                return myCal.GetWeekOfYear(date, myCWR, myFirstDOW); 
            }

            if (ModelState.IsValid)
            {
                obj.WeekNumber = GetWeekNumber(now);
                _unitOfWork.Habit.Add(obj);
                _unitOfWork.Save();
                date = mondayDate;

                for (int i = 0; i < 7; i++)
                {
                    HabitRealization habitDay = new HabitRealization();
                    habitDay.Date = date;
                    date = date.AddDays(1);
                    habitDay.HabitId = obj.Id;
                    _unitOfWork.HabitRealization.Add(habitDay);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Habit created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Week()
        {
            List<Habit> habits = _unitOfWork.Habit.GetAll(includeProperties: "habitRealizations").ToList();
            List<int> weeks = new List<int>();

            foreach (Habit hab in habits)
            {
                if(weeks.LastOrDefault()!=hab.WeekNumber)
                weeks.Add(hab.WeekNumber);
            }

            return View(weeks);
        }
    }
}
