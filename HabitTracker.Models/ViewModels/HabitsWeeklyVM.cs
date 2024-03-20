using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class HabitsWeeklyVM
    {
        public List<Habit> habits { get; set; }
        public List<HabitRealization> habitRealizations { get; set; }
    }
}
