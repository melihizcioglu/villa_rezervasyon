using Microsoft.EntityFrameworkCore;
using VillaRezervasyonApi.Models;

namespace VillaRezervasyonApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<BoatReservation> BoatReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Villa Features configuration
            modelBuilder.Entity<Villa>()
                .Property(v => v.Features)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            // Boat Features configuration
            modelBuilder.Entity<Boat>()
                .Property(b => b.Features)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            // Configure relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Villa)
                .WithMany()
                .HasForeignKey(r => r.VillaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoatReservation>()
                .HasOne(r => r.Boat)
                .WithMany()
                .HasForeignKey(r => r.BoatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Villa>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Boat>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
} 