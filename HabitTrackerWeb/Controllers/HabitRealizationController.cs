using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Repositry;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HabitTrackerWeb.Controllers
{
    public class HabitRealizationController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public HabitRealizationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult HabitsWeekly()
        {
            List<Habit> habits = _unitOfWork.Habit.GetAll(includeProperties: "habitRealizations").ToList();

            return View(habits);
        }

        [HttpGet]
        public IActionResult UpdateIfExecuted(int id)
        {
            try
            {
                var item = _unitOfWork.HabitRealization.Get(u => u.Id == id);
                if (item == null)
                {
                    return NotFound();
                }

                if (item.IfExecuted == 0)
                {
                    item.IfExecuted = 1;
                }
                else if (item.IfExecuted == 1)
                {
                    item.IfExecuted = 2;
                }
                else item.IfExecuted = 0;

                _unitOfWork.HabitRealization.Update(item);
                _unitOfWork.Save();

                return Json(new { ifExecuted = item.IfExecuted });
            }
            catch (Exception ex)
            {
                // Jeśli wystąpił błąd, zwróć odpowiedni kod błędu
                return new StatusCodeResult(404);
            }
        }
    }
}
