using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerWeb.Controllers
{
    public class HabitRealizationController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public HabitRealizationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<HabitRealization> objHabitRealizationList = _unitOfWork.HabitRealization.GetAll().ToList();
            return View(objHabitRealizationList);
        }
    }
}
