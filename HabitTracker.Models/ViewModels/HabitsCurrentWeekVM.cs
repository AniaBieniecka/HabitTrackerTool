using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class HabitsCurrentWeekVM
    {
        public List<Habit> habits { get; set; }
        public bool habitsHasAnyData { get; set; }
    }
}
