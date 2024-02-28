using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            DateOnly Today = DateOnly.FromDateTime(now);
            DayOfWeek WeekDayToday = Today.DayOfWeek;

            DateOnly startOfWeek = Today.AddDays(-(int)Today.DayOfWeek); // - Monday date

            if (ModelState.IsValid)
            {
                _unitOfWork.Habit.Add(obj);
                _unitOfWork.Save();

                for (int i = 0; i < 7; i++)
                {
                    HabitRealization habitDay = new HabitRealization();
                    habitDay.Date = startOfWeek;
                    startOfWeek = startOfWeek.AddDays(1);
                    habitDay.HabitId = obj.Id;
                    _unitOfWork.HabitRealization.Add(habitDay);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Habit created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
