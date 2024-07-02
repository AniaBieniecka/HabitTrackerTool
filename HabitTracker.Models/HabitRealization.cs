
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
        public int IfExecuted { get; set; } = 0;

        //0 - false, 1 - true, 2 - partially executed
        public DateOnly Date { get; set; }


        public int HabitWeekId { get; set; }
        [ForeignKey("HabitWeekId")]
        public HabitWeek habitWeek { get; set; }
    }
}
