using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Data;
using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HabitTracker.Models.ScoringModels;

namespace HabitTracker.DataAccess.Repository
{
    public class ScoreRepository : Repository<Score>, IScoreRepository

    {
        private ApplicationDbContext _db;
        public ScoreRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(Score obj)
        {
            _db.Scores.Update(obj);
        }
    }
}
