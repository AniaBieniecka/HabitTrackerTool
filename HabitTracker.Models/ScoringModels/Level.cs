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

namespace HabitTracker.Models.ScoringModels
{
    public class Level
    {
        [NotMapped]
        public int Id { get; set; }
        [NotMapped]
        public int MinimumScore { get; set; }

        public Level(int id, int minimumScore)
        {
            Id = id;
            MinimumScore = minimumScore;
        }

    }
}
