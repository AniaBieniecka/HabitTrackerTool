
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
        public int DaysRangeId { get; set; }
        [ForeignKey("DaysRangeId")]
        public TimePeriod daysTracked { get; set; }
    }
}
