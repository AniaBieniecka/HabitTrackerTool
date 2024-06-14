using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models.ViewModels
{
    public class ProgressChartVM
    {
        public List<ChartData> Series { get; set; }
        public DateOnly[] Categories { get; set; }
    }

    public class ChartData
    {
        public string Name { get; set; }
        public float[] Data { get; set; }
    }
}
