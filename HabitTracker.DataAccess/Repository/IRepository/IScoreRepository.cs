using HabitTracker.Models;
using HabitTracker.Models.ScoringModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IScoreRepository: IRepository<Score>
    {
        void Update(Score obj);

    }
}
