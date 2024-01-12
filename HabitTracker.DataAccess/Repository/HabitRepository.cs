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
    public class HabitRepository : Repository<Habit>, IHabitRepository

    {
        private ApplicationDbContext _db;
        public HabitRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(Habit obj)
        {
            _db.Habits.Update(obj);
        }
    }
}
