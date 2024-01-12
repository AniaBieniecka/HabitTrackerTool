
using HabitTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HabitTracker.DataAccess.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Habit> Habits {  get; set; }
        public DbSet<TimePeriod> TimePeriods { get; set; }
        public DbSet<HabitRealization> HabitRealizations { get; set; }


    }


}
