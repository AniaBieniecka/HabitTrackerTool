using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Repositry;
using HabitTracker.Models;
using HabitTracker.Models.ViewModels;
using HabitTrackerWeb.Controllers.Services;
using HabitTrackerWeb.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace HabitTrackerWeb.Controllers
{
    [Authorize]
    public class HabitWeekController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateService _dateService;
        private readonly UserManager<IdentityUser> _userManager;

        public HabitWeekController(IUnitOfWork unitOfWork, IDateService dateService, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _dateService = dateService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            List<Habit> objHabitList = _unitOfWork.Habit.GetAll(u=>u.UserId==userId,includeProperties:"habitWeeks").ToList();
            return View(objHabitList);
        }


        public IActionResult Upsert(int? id)
        {
            Habit habit = new Habit();
            HabitWeek habitWeek = new HabitWeek();

            if (id != null)
            {
                habitWeek = _unitOfWork.HabitWeek.Get(u => u.Id == id, includeProperties: "habit");
                habit = _unitOfWork.Habit.Get(u => u.Id == habitWeek.HabitId);
            }

            return View(habitWeek);
        }

        [HttpPost]
        public IActionResult Upsert(HabitWeek habitWeek)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var habitsFromDB = _unitOfWork.Habit.GetAll();
            int habitId;
            bool isHabitFoundInDB = false;

            DateOnly mondayDate = _dateService.LastMonday();
            DateOnly loopDate = mondayDate;
            int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var existingHabitWeek = _unitOfWork.HabitWeek.Get
                (u => u.WeekNumber == currentWeek && u.Year == mondayDate.Year && u.habit.Name == habitWeek.habit.Name);

            if (existingHabitWeek != null)
            {
                ModelState.AddModelError("habit.Name", "Habit with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                //CREATE
                if (habitWeek.HabitId == 0)
                {
                    //CHECK IF HABIT WITH THE SAME NAME EXIST
                    foreach (var habitFromDB in habitsFromDB)
                    {
                        //IF EXIST DONT CREATE NEW HABIT
                        if (habitFromDB.Name == habitWeek.habit.Name)
                        {
                            isHabitFoundInDB = true;

                            //CHECK IF HABITWEEK EXIST, IF NOT THEN CREATE HABITWEEK
                            if (habitWeek.Id == 0)
                            {
                                habitWeek.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                                habitWeek.Year = loopDate.Year;
                                habitWeek.HabitId = habitFromDB.Id;
                                _unitOfWork.HabitWeek.Add(habitWeek);
                                _unitOfWork.Save();
                                var habitWeekId = habitWeek.Id;

                                //AND CREATE HABIT REALIZATIONS
                                for (int i = 0; i < 7; i++)
                                {
                                    HabitRealization habitDay = new HabitRealization();
                                    habitDay.Date = loopDate;
                                    loopDate = loopDate.AddDays(1);
                                    habitDay.HabitWeekId = habitWeekId;
                                    _unitOfWork.HabitRealization.Add(habitDay);
                                    _unitOfWork.Save();
                                }
                            }
                            //ELSE (IF HABITWEEK EXIST) UPDATE HABITWEEK
                            else
                            {
                                _unitOfWork.HabitWeek.Update(habitWeek);
                                _unitOfWork.Save();

                            }

                            break;
                        }

                    }
                    // IF NOT FOUND IN DB - CREATE HABIT, HABITWEEK AND HABITREALIZATIONS
                    if (!isHabitFoundInDB)
                    {
                        habitWeek.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                        habitWeek.Year = loopDate.Year;
                        habitWeek.habit.UserId = userId;
                        _unitOfWork.Habit.Add(habitWeek.habit);
                        _unitOfWork.Save();

                        habitWeek.HabitId = habitWeek.habit.Id;
                        _unitOfWork.HabitWeek.Add(habitWeek);
                        _unitOfWork.Save();

                        for (int i = 0; i < 7; i++)
                        {
                            HabitRealization habitDay = new HabitRealization();
                            habitDay.Date = loopDate;
                            loopDate = loopDate.AddDays(1);
                            habitDay.HabitWeekId = habitWeek.Id;
                            _unitOfWork.HabitRealization.Add(habitDay);
                            _unitOfWork.Save();
                        }
                    }
                }

                //UPDATE HABIT AND HABITWEEK
                else
                {

                    var habitWeekToUpdate = _unitOfWork.HabitWeek.Get(w => w.Id == habitWeek.Id);
                    habitWeekToUpdate.WeeklyGoal = habitWeek.WeeklyGoal;
                    _unitOfWork.HabitWeek.Update(habitWeekToUpdate);
                    _unitOfWork.Save();

                    //var habitToUpdate = _unitOfWork.Habit.Get(u => u.Id == habitWeek.HabitId);
                    habitWeek.habit.Id = habitWeek.HabitId;

                    _unitOfWork.Habit.Update(habitWeek.habit);
                    _unitOfWork.Save();
                }

                return RedirectToAction("HabitsCurrentWeek", "HabitRealization");

            }
            return View(habitWeek);
        }


        public IActionResult ShowWeeks()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitWeek> habitWeeks = _unitOfWork.HabitWeek.GetAll(u=>u.habit.UserId == userId).ToList();

            List<WeekYear> weekYearList = new List<WeekYear>();

            foreach (var hab in habitWeeks)
            {
                var weekYear = new WeekYear { Week = hab.WeekNumber, Year = hab.Year };
                weekYearList.Add(weekYear);
            }

            var uniqueWeekYearList = weekYearList.Distinct().ToList();

            return View(uniqueWeekYearList);
        }
        public IActionResult ChooseHabits(int? habitsCount)
        {
            var habitSuggestion = new HabitSuggestion();
            var habitSuggestionToDisplay = new List<string>();
            var indexList = new List<int>();
            int counter = 0;

            Random rnd = new Random();
            while (indexList.Count < 3 )
            {
                int index = rnd.Next(habitSuggestion.HabitSuggestions.Count - 1);
                counter++;

                if (!indexList.Contains(index))
                {
                    var habitInDB = _unitOfWork.Habit.GetAll().FirstOrDefault(hab => hab.Name == habitSuggestion.HabitSuggestions[index]);

                    if (habitInDB is null)
                    {
                        indexList.Add(index);
                        habitSuggestionToDisplay.Add(habitSuggestion.HabitSuggestions[index]);
                    }
                }

                if (counter >= 15)
                {
                    break; 
                }
            }

            return View(habitSuggestionToDisplay);
        }

        [HttpPost]
        public IActionResult ChooseHabits(string[] habitNames)
        {
            DateOnly mondayDate = _dateService.LastMonday();
            DateOnly loopDate;
            var habitsFromDB = _unitOfWork.Habit.GetAll();
            int habitId;
            bool isHabitFoundInDB = false;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            foreach (var habitName in habitNames)
            {

                loopDate = mondayDate;

                foreach (var habitFromDB in habitsFromDB)
                {
                    //CHECK IF HABIT WITH THIS NAME ALREADY EXIST
                    if (habitFromDB.Name == habitName)
                    {
                        isHabitFoundInDB = true;
                        var currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                        //CHECK IF HABIT WEEK FOR THIS HABIT AND WEEK EXIST

                        var existingHabitWeek = _unitOfWork.HabitWeek.GetAll
                        (u => u.WeekNumber == currentWeek && u.Year == mondayDate.Year && u.habit.Name == habitName && u.habit.UserId == userId);

                        //IF HABIT WEEK EXIST
                        if (existingHabitWeek != null)
                        {
                            ModelState.AddModelError("habit.Name", "Habit with this name already exists.");
                        }
                        else //CREATE HABIT WEEK AND HABIT REALIZATIONS
                        {
                            HabitWeek habitWeek = new HabitWeek();
                            habitWeek.HabitId = habitFromDB.Id;
                            habitWeek.WeekNumber = currentWeek;
                            habitWeek.Year = loopDate.Year;
                            habitWeek.WeeklyGoal = 7;

                            _unitOfWork.HabitWeek.Add(habitWeek);
                            _unitOfWork.Save();
                            var habitWeekId = habitWeek.Id;

                            for (int i = 0; i < 7; i++)
                            {
                                HabitRealization habitDay = new HabitRealization();
                                habitDay.Date = loopDate;
                                loopDate = loopDate.AddDays(1);
                                habitDay.HabitWeekId = habitWeekId;
                                _unitOfWork.HabitRealization.Add(habitDay);
                                _unitOfWork.Save();
                            }
                        }
                    }
                }

                if (!isHabitFoundInDB)
                {
                    Habit habit = new Habit();
                    habit.Name = habitName;
                    habit.UserId = userId;

                    _unitOfWork.Habit.Add(habit);
                    _unitOfWork.Save();

                    HabitWeek habitWeek = new HabitWeek();
                    habitWeek.WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    habitWeek.Year = loopDate.Year;
                    habitWeek.HabitId = habit.Id;
                    habitWeek.WeeklyGoal = 7;

                    _unitOfWork.HabitWeek.Add(habitWeek);
                    _unitOfWork.Save();

                    for (int i = 0; i < 7; i++)
                    {
                        HabitRealization habitDay = new HabitRealization();
                        habitDay.Date = loopDate;
                        loopDate = loopDate.AddDays(1);
                        habitDay.HabitWeekId = habitWeek.Id;
                        _unitOfWork.HabitRealization.Add(habitDay);
                        _unitOfWork.Save();
                    }
                }
            }
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }

        public IActionResult CreateHabitsFromPreviousWeek()
        {
            int weekCurrent = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int weekPrevious = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            DateOnly lastMondayDate = _dateService.LastMonday();
            DateOnly mondayDateFromPreviousWeek = lastMondayDate.AddDays(-7);
            int year = mondayDateFromPreviousWeek.Year;
            DateOnly loopDate;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<HabitWeek> habitWeeksFromPreviousWeek = _unitOfWork.HabitWeek.GetAll(u => u.WeekNumber == weekPrevious && u.Year == year && u.habit.UserId == userId).ToList();


            if (habitWeeksFromPreviousWeek.Count > 0)
            {
                foreach (var habitWeekPreviousWeek in habitWeeksFromPreviousWeek)
                {
                    HabitWeek newHabitWeek = new();
                    newHabitWeek.Year = DateTime.Now.Year;
                    newHabitWeek.WeekNumber = weekCurrent;
                    newHabitWeek.WeeklyGoal = habitWeekPreviousWeek.WeeklyGoal;
                    newHabitWeek.HabitId = habitWeekPreviousWeek.HabitId;
                    _unitOfWork.HabitWeek.Add(newHabitWeek);
                    _unitOfWork.Save();

                    loopDate = lastMondayDate;

                    for (int i = 0; i < 7; i++)
                    {
                        HabitRealization habitDay = new HabitRealization();
                        habitDay.Date = loopDate;
                        loopDate = loopDate.AddDays(1);
                        habitDay.HabitWeekId = newHabitWeek.Id;
                        _unitOfWork.HabitRealization.Add(habitDay);
                        _unitOfWork.Save();
                    }
                }
                return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
            }
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            HabitWeek? habitWeekFromDB = _unitOfWork.HabitWeek.Get(u => u.Id == id, includeProperties: "habit");
            if (habitWeekFromDB == null)
            {
                return NotFound();
            }
            return View(habitWeekFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            HabitWeek? habitWeekFromDB = _unitOfWork.HabitWeek.Get(u => u.Id == id);

            if (habitWeekFromDB == null)
            {
                return NotFound();
            }

            _unitOfWork.HabitWeek.Remove(habitWeekFromDB);
            _unitOfWork.Save();

            Habit habitFromDB = _unitOfWork.Habit.Get(u => u.Id == habitWeekFromDB.HabitId, includeProperties: "habitWeeks");

            if (habitFromDB.habitWeeks.Count() ==0)
            {
                _unitOfWork.Habit.Remove(habitFromDB);
                _unitOfWork.Save();

            }
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("HabitsCurrentWeek", "HabitRealization");
        }
    }
}

