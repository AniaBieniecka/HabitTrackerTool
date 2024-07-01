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
            List<Habit> objHabitList = _unitOfWork.Habit.GetAll(includeProperties: "habitWeeks").ToList();
            return View(objHabitList);
        }


        public IActionResult Update(int? id)
        {
            Habit habit = new Habit();
            if (id != null)
            {
                habit = _unitOfWork.Habit.Get(u => u.Id == id);
            }
            return View(habit);
        }

        [HttpPost]
        public IActionResult Update(Habit habit)
        {
            if (ModelState.IsValid)
            {

                Habit habitToUpdate = _unitOfWork.Habit.Get(u => u.Id == habit.Id);
                habitToUpdate.Name = habit.Name;
                _unitOfWork.Habit.Update(habit);
                _unitOfWork.Save();
                return RedirectToAction("HabitsCurrentWeek", "HabitRealization");

            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Habit? habitFromDB = _unitOfWork.Habit.Get(u => u.Id == id);
            if (habitFromDB == null)
            {
                return NotFound();
            }
            return View(habitFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Habit? habitFromDB = _unitOfWork.Habit.Get(u => u.Id == id);
            if (habitFromDB == null)
            {
                return NotFound();
            }

            _unitOfWork.Habit.Remove(habitFromDB);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }
    }
}

