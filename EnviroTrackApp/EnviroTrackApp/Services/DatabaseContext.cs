using EnviroTrackApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnviroTrackApp.Services
{
    public class DatabaseContext : DbContext
    {
        public DbSet<GoalModel> Goals { get; set; }

        public DatabaseContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Goal.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}