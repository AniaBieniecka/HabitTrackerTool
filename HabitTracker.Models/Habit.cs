using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public int Name { get; set; }
            
    }
}
