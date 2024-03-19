using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Repositry;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateTime mondayDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            int year = DateOnly.FromDateTime(mondayDate).Year;

            List<Habit> habits = _unitOfWork.Habit.GetAll(u => u.WeekNumber == weekCurrent && u.Year == year, includeProperties: "habitRealizations").ToList();

            HabitsCurrentWeekVM habitsCurrentWeekVM = new HabitsCurrentWeekVM();
           
            bool habitsHasAnyData = _unitOfWork.Habit.HasAnyData();
            habitsCurrentWeekVM.habitsHasAnyData = habitsHasAnyData;
            habitsCurrentWeekVM.habits = habits;

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
                return new StatusCodeResult(404);
            }
        }
    }
}
