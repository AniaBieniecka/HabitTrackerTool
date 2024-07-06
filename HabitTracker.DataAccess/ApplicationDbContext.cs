
using HabitTracker.Models;
using HabitTracker.Models.ScoringModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HabitTracker.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Habit> Habits {  get; set; }
        public DbSet<HabitWeek> HabitWeeks { get; set; }
        public DbSet<HabitRealization> HabitRealizations { get; set; }
        public DbSet<ViewSetting> ViewSettings { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }


}
