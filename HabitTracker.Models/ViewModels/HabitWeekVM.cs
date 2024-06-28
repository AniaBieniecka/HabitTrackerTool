using HabitTracker.Models.ScoringModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class HabitWeekVM
    {
        public HabitWeek habitWeek { get; set; }

        public string habitName { get; set; }
        public int habitID { get; set; }
    }
}
