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
    public class Score
    {
        public int Id { get; set; }

        [DisplayName("Score value")]
        public int ScoreValue { get; set; }

        public int LevelId { get; set; }
        [ForeignKey("Id")]
        public Level level{ get; set; }
        public Score()
        {
            ScoreValue = 0;
        }

    }
}
