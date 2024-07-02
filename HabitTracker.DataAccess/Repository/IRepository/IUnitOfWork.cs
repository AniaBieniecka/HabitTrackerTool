using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitTracker.Models;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHabitRepository Habit { get; }
        IHabitWeekRepository HabitWeek { get;}
        IHabitRealizationRepository HabitRealization { get; }
        IViewSettingRepository ViewSetting { get; }
        IScoreRepository Score { get; }
        void Save();
    }
}
