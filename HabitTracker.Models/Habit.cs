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
    public class Habit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("How often per week")]
        [Range(1, 7, ErrorMessage = "Wartość musi być większa lub równa 1, ale mniejsza od 7.")]

        //planned quantity of habits per week
        public int WeeklyGoal { get; set; }

        [DisplayName("Week number")]
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public bool IsWeeklyGoalAchieved { get; set; }
        public Habit()
        {
            WeeklyGoal = 7;
            IsWeeklyGoalAchieved = false;
        }

        [ValidateNever]
        public List<HabitRealization> habitRealizations { get; set; }

        [ValidateNever]
        [NotMapped]
        public ViewSetting ViewSetting { get; set; }

    }
}
