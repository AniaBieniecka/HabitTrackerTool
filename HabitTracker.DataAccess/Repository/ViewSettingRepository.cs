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
    public class ViewSettingRepository : Repository<ViewSetting>, IViewSettingRepository

    {
        private ApplicationDbContext _db;
        public ViewSettingRepository(ApplicationDbContext db): base(db)
        {
            _db = db;   
        }


        public void Update(ViewSetting obj)
        {
            _db.ViewSettings.Update(obj);
        }
    }
}
