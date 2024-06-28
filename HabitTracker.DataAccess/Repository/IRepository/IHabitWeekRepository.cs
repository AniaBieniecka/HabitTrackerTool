using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IHabitWeekRepository: IRepository<HabitWeek>
    {
        void Update(HabitWeek obj);

    }
}
