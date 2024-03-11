using Microsoft.EntityFrameworkCore;

using Core.Entities;


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

        #region Overriding
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql("Host=postgres;Port=5432;Database=db;Username=postgres;Password=FDgsd32Fkfsj2kfkv11");

            base.OnConfiguring(builder);
        }
        #endregion
    }
}
