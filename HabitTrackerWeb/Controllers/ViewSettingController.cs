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
            var avaialableColor = new AvailableColor();
            var viewSetting = new ViewSetting()
            {
                availableIconsDone = availableIcon.AvailableIconsDone,
                availableIconsPartiallyDone = availableIcon.AvailableIconsPartiallyDone,
                availableColors = avaialableColor.AvailableColors
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

            List<Habit> habits = _unitOfWork.Habit.GetAll(includeProperties: "habitRealizations").ToList();
            return RedirectToAction("Views/HabitRealization/HabitsWeekly.cshtml", habits);
        }
    }
}
