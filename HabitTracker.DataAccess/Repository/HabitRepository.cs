using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.DataAccess.Data;
using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.DataAccess.Repository
{
    public class HabitRepository : Repository<Habit>, IHabitRepository

    {
        private ApplicationDbContext _db;
        public HabitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Habit obj)
        {
            var local = _db.Set<Habit>()
        .Local
        .FirstOrDefault(entry => entry.Id.Equals(obj.Id));

            if (local != null)
            {
                _db.Entry(local).State = EntityState.Detached;
            }

            _db.Entry(obj).State = EntityState.Modified;

        }
    }
}
