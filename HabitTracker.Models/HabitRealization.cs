
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    public class HabitRealization
    {
        public int Id { get; set; }
        public bool IfExecuted { get; set; } = false;
        public DateOnly Date { get; set; }
        public int HabitId { get; set; }
        [ForeignKey("HabitId")]
        public Habit habit { get; set; }
    }
}
