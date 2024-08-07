﻿using HabitTracker.DataAccess.Repository.IRepository;
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
    public class HabitWeekRepository : Repository<HabitWeek>, IHabitWeekRepository

    {
        private ApplicationDbContext _db;
        public HabitWeekRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(HabitWeek obj)
        {
            _db.HabitWeeks.Update(obj);
        }
    }
}
