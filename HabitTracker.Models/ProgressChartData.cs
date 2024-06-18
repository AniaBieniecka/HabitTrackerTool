using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class ProgressChartData
    {
        public DateOnly Date {  get; set; }
        public float Value { get; set; }
    }
}
