﻿using System.Threading.Tasks;

using Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Core
{
    public sealed class ApplicationContext : DbContext
    {
        #region Properties
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<HumanName> HumanNames { get; set; } = null!;
        #endregion

        #region Constructors
        public ApplicationContext()
        {
            this.Database.EnsureCreated();
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Port=5432;Database=db;Username=postgres;Password=FDgsd32Fkfsj2kfkv11");

            base.OnConfiguring(options);
        }
    }
}
