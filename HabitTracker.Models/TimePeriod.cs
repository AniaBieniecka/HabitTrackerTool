
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    public class TimePeriod
    {
        public int Id {  get; set; }

        public int LevelId  { get; set; }

        [DisplayName("Level name")]
        public string? LevelName  { get; set; }
        DateOnly StartDate { get; set; }
        DateOnly EndDate { get; set; }
        public int HabitId { get; set; }
        [ForeignKey("HabitId")]
        public Habit habit { get; set; }
    }
}
