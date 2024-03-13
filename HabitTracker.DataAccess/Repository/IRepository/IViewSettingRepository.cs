using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IViewSettingRepository : IRepository<ViewSetting>
    {
        void Update(ViewSetting obj);

    }
}
