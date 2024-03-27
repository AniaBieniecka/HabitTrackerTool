
using HabitTracker.DataAccess.Data;
using HabitTracker.DataAccess.Repository;
using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repositry
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IHabitRepository Habit { get; set; }
        public IHabitRealizationRepository HabitRealization { get; set; }
        public IViewSettingRepository ViewSetting { get; set; }
        public IScoreRepository Score { get; set; }

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Habit = new HabitRepository(_db);
            HabitRealization = new HabitRealizationRepository(_db);
            ViewSetting = new ViewSettingRepository(_db);
            Score = new ScoreRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
