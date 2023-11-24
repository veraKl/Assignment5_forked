using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.Models;

namespace MVC_EF_Start.DataAccess
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Range> Ranges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // State and County relationship
            modelBuilder.Entity<County>()
                .HasOne(c => c.State)
                .WithMany(s => s.Counties)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // County and Vehicle relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.CountyNavigation)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CountyId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            // State and Vehicle relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.StateNavigation)
                .WithMany(s => s.Vehicles)
                .HasForeignKey(v => v.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Make and Model relationship
            modelBuilder.Entity<Model>()
                .HasOne(m => m.Make)
                .WithMany(mk => mk.Models)
                .HasForeignKey(m => m.MakeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Make and Vehicle relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.MakeNavigation)
                .WithMany(mk => mk.Vehicles)
                .HasForeignKey(v => v.MakeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Model and Vehicle relationship
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.ModelNavigation)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Range and Vehicle relationship
            modelBuilder.Entity<Range>()
                .HasOne(r => r.Vehicle)
                .WithOne(v => v.RangeNavigation)
                .HasForeignKey<Range>(r => r.VinNumber)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
        


    }
}