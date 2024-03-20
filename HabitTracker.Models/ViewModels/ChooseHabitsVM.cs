using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class ChooseHabitsVM
    {
        public List<Habit> habits { get; set; }
        public List<string> habitSuggestions { get; set; }
    }
}
