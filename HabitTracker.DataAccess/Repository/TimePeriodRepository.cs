using HabitTracker.DataAccess.Repository.IRepository;
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
    public class TimePeriodRepository : Repository<TimePeriod>, ITimePeriodRepository

    {
        private ApplicationDbContext _db;
        public TimePeriodRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(TimePeriod obj)
        {
            _db.TimePeriods.Update(obj);
        }
    }
}
