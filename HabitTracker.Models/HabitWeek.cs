using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    public class HabitWeek
    {
        public int Id { get; set; }

        [DisplayName("Week number")]
        public int WeekNumber { get; set; }
        public int Year { get; set; }

        [DisplayName("How often per week")]
        [Range(1, 7, ErrorMessage = "Wartość musi być większa lub równa 1, ale mniejsza od 7.")]

        //planned quantity of habits per week
        public int WeeklyGoal { get; set; }
        public bool IsWeeklyGoalAchieved { get; set; }

        public HabitWeek()
        {
            WeeklyGoal = 7;
            IsWeeklyGoalAchieved = false;
        }
        public int HabitId { get; set; }
        [ForeignKey("HabitId")]
        [ValidateNever]
        public Habit habit { get; set; }

        [ValidateNever]
        public List<HabitRealization> habitRealizations { get; set; }
    }
}
