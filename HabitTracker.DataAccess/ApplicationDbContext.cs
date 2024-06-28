
using HabitTracker.Models;
using HabitTracker.Models.ScoringModels;
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
        public DbSet<HabitWeek> HabitWeeks { get; set; }
        public DbSet<HabitRealization> HabitRealizations { get; set; }
        public DbSet<ViewSetting> ViewSettings { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewSetting>().HasData(new ViewSetting { Id = 1, Color = "00CED1", IconPartiallyDone = "bi bi-check-square", IconDone = "bi bi-check-square-fill" }); 
            modelBuilder.Entity<Score>().HasData(new Score { Id = 1, ScoreValue = 0 }); 

        }

    }


}
