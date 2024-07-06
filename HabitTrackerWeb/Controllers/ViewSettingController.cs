using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Security.Claims;

namespace HabitTrackerWeb.Controllers
{
    [Authorize]
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

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewSettingFromDB = _unitOfWork.ViewSetting.Get(u => u.UserId == userId);

            if (viewSettingFromDB != null)
            {
                viewSettingFromDB.Color = viewSetting.Color;
                viewSettingFromDB.IconPartiallyDone = viewSetting.IconPartiallyDone;
                viewSettingFromDB.IconDone = viewSetting.IconDone;

                if (ModelState.IsValid)
                {
                    _unitOfWork.ViewSetting.Update(viewSettingFromDB);
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
