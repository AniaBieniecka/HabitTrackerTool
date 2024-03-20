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
    public class HabitSuggestion
    {
        public object habitSuggestions;

        public List<string> HabitSuggestions { get; set; }

        public HabitSuggestion()
        {
            HabitSuggestions = new List<string>
        {

            "Make your bed",
            "Express gratitude for 3 things in your life",
            "Clean for 15 minute",
            "Write a journal",
            "Take a walk",
            "Make something nice for someone",
            "Enjoy a pleasant moment carefully",
            "Don't use your phone before sleep",
            "Don't eat sugar",
            "Get enough sleep",
            "Don't use social media"
        };
        }
    }
}
