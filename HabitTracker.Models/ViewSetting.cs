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

namespace HabitTracker.Models
{
    public class ViewSetting
    {
        public int Id { get; set; }

        public string Color { get; set; }

        public string IconPartiallyDone { get; set; }
        

        [ValidateNever]
        public string IconDone { get; set; }

        [ValidateNever]
        [NotMapped]
        public List<string> availableColors { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<string> availableIconsDone { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<string> availableIconsPartiallyDone { get; set; }


    }
}
