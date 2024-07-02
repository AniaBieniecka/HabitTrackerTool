using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace HabitTrackerWeb.Controllers
{
    public class ViewSettingController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;


        public ViewSettingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Update()
        {
            var availableIcon = new AvailableIcon();
            var availableColor = new AvailableColor();
            var viewSetting = new ViewSetting()
            {
                availableIconsDone = availableIcon.AvailableIconsDone,
                availableIconsPartiallyDone = availableIcon.AvailableIconsPartiallyDone,
                availableColors = availableColor.AvailableColors
            };


            return View(viewSetting);
        }

        [HttpPost]
        public IActionResult Update(ViewSetting viewSetting)
        {

            viewSetting.IconDone = viewSetting.IconPartiallyDone + "-fill";

            if (_unitOfWork.ViewSetting.Get(u => u.Id == 1) != null)
            {
                viewSetting.Id = 1;
                if (ModelState.IsValid)
                {
                    _unitOfWork.ViewSetting.Update(viewSetting);
                    _unitOfWork.Save();
                    TempData["success"] = "Viewsetting updated successfully";
                }
            }

            List<Habit> habits = _unitOfWork.Habit.GetAll().ToList();

            foreach (Habit hab in habits)
            {
                hab.ViewSetting = viewSetting;
            }
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");

        }
    }
}
