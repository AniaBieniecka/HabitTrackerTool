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
            if (ModelState.IsValid)
            {
                _unitOfWork.Habit.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
