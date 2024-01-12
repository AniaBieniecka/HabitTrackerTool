using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using HabitTracker.DataAccess.Data;
using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository
{
    public class HabitRealizationRepository : Repository<HabitRealization>, IHabitRealizationRepository

    {
        private ApplicationDbContext _db;
        public HabitRealizationRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(HabitRealization obj)
        {
            _db.HabitRealizations.Update(obj);
        }
    }
}
