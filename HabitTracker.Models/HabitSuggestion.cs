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
            "Practise gratitude",
            "Clean for 15 minutes",
            "Write a journal for 5 minutes",
            "Take a walk",
            "Make a one good things for someone",
            "Practise mindfulness",
            "Don't use your phone before sleep",
            "Don't eat sugar",
            "Get enough sleep",
            "Don't use social media for 8 hours daily",
            "Read 5 pages of the book",
            "Exercise for 10 minutes",
            "Learn 5 new words in foreign language",
            "Eat healthy",
            "Take a vitamin D"
        };
        }
    }
}
