using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTrackerWeb.Controllers.Services;
using HabitTrackerWeb.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace HabitTrackerWeb.Controllers
{
    [Authorize]
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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Habit> objHabitList = _unitOfWork.Habit.GetAll(u=>u.UserId == userId, includeProperties: "habitWeeks").ToList();
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
                habit.UserId = habitToUpdate.UserId;
                _unitOfWork.Habit.Update(habit);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Habit");

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
            return RedirectToAction("Index", "Habit");
        }
    }
}

