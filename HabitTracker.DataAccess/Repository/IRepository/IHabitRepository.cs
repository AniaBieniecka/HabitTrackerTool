using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IHabitRepository: IRepository<Habit>
    {
        void Update(Habit obj);

    }
}
