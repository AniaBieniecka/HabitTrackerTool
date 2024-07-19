using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTrackerWeb.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HabitTrackerWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationService _applicationService;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IApplicationService applicationService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _applicationService = applicationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AboutScoring()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var habitsCurrentWeekVM = new HabitsCurrentWeekVM
            {
                numberOfWeeks = _applicationService.HowManyWeeks(userId),
                score = _unitOfWork.Score.Get(u => u.UserId == userId),
            };
            return View(habitsCurrentWeekVM);
        }
    }
}
