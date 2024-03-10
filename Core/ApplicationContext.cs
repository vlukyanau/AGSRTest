using System.Threading.Tasks;

using Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Core
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<HumanName> HumanNames { get; set; } = null!;

        public ApplicationContext()
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Port=5432;Database=db;Username=postgres;Password=FDgsd32Fkfsj2kfkv11");
        }
    }
}
