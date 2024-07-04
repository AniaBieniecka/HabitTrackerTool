using HabitTracker.Models.ScoringModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class HabitsCurrentWeekVM
    {
        public List<HabitWeek> habitWeekList { get; set; }
        public int howManyHabitsInPreviousWeek { get; set; }
        public Score score { get; set; }
        public int numberOfWeeks { get; set; }

    }
}
