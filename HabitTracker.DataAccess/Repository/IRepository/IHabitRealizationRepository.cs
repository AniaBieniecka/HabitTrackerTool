using HabitTracker.Models;
using HabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.DataAccess.Repository.IRepository
{
    public interface IHabitRealizationRepository: IRepository<HabitRealization>
    {

        void Update(HabitRealization obj);

    }
}
