using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Models
{
    public class AvailableColor
    {
        public List<string> AvailableColors { get; set; }

        public AvailableColor()
        {
            AvailableColors = new List<string>
        {

            "#00CED1",
            "#ADD8E6",
            "#87CEFA",
            "#00BFFF",
            "#9400D3",
            "#FF1493",
            "#FF69B4",
            "#F08080",
            "#FF6347",
            "red",
            "#FFE400",
            "#006400",
            "#483D8B",
            "black"
        };
        }

    }
}
