using Microsoft.EntityFrameworkCore;
using DataLayer_NRE_Portal.Models;

namespace DataLayer_NRE_Portal.Data
{
    public class NrePortalContext : DbContext
    {
        // Primary ctor used at runtime via DI
        public NrePortalContext(DbContextOptions<NrePortalContext> options) : base(options) { }

        // Parameterless ctor for design-time fallback
        public NrePortalContext() { }

        public virtual DbSet<ProductionData> ProductionSummaries { get; set; } = null!;
        public virtual DbSet<InstallationBase> Installations => Set<InstallationBase>();
        public virtual DbSet<PublicInstallation> PublicInstallations => Set<PublicInstallation>();
        public virtual DbSet<PrivateInstallation> PrivateInstallations => Set<PrivateInstallation>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // If no provider configured (design-time), use local SQLite
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=/tmp/nreportal.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstallationBase>().ToTable("Installations");
            modelBuilder.Entity<PublicInstallation>().ToTable("PublicInstallations");
            modelBuilder.Entity<PrivateInstallation>().ToTable("PrivateInstallations");

            modelBuilder.Entity<InstallationBase>().HasIndex(x => new { x.Region, x.EnergyType });
            modelBuilder.Entity<InstallationBase>().HasIndex(x => new { x.Latitude, x.Longitude });
        }
    }
}
