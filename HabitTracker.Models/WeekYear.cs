using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HabitTracker.Models
{
    public class WeekYear
    {
        [NotMapped]
        public int Week  { get; set; }
        [NotMapped]
        public int Year { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            WeekYear other = (WeekYear)obj;
            return this.Week == other.Week && this.Year == other.Year;
        }

        public override int GetHashCode()
        {
            return Week.GetHashCode() ^ Year.GetHashCode();
        }
    }
}
