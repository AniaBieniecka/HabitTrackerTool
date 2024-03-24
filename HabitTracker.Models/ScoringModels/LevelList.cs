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
    public class LevelList
    {
        public List<Level> Levels { get; set; }

        public LevelList()
        {
            Levels = new List<Level>
        {

            new Level(1, 0),
            new Level(2, 200),
            new Level(3, 500),
            new Level(4, 1000),
            new Level(5, 1500),
            new Level(6, 2000),
            new Level(7, 2500),
            new Level(8, 3000),
        };
        }

    }
}
