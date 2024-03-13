
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
        public DbSet<HabitRealization> HabitRealizations { get; set; }

        public DbSet<ViewSetting> ViewSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewSetting>().HasData(new ViewSetting { Id = 1, Color = "00CED1", IconPartiallyDone = "bi bi-check-square", IconDone = "bi bi-check-square-fill" });

        }

    }


}
