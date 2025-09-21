using carparkinginterview.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace carparkinginterview.Data
{
    public class CarParkContext : DbContext
    {
        public CarParkContext(DbContextOptions<CarParkContext> options) : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
               .HasOne(v => v.ParkingSpace)
               .WithMany()
               .HasForeignKey(v => v.ParkingSpaceId)
               .OnDelete(DeleteBehavior.Restrict);
        }

        public void SeedData()
        {
            //Database.EnsureDeleted();
            // Ensure database and tables are created first
            Database.EnsureCreated();
            // Add 100 parking spaces (20 small, 50 medium, 30 large)
            if (!ParkingSpaces.Any())
            {
                var spaces = new List<ParkingSpace>();

                for (int i = 1; i <= 20; i++)
                {
                    spaces.Add(new ParkingSpace { Size = "Small", IsOccupied = false });
                }

                for (int i = 21; i <= 70; i++)
                {
                    spaces.Add(new ParkingSpace { Size = "Medium", IsOccupied = false });
                }

                for (int i = 71; i <= 100; i++)
                {
                    spaces.Add(new ParkingSpace { Size = "Large", IsOccupied = false });
                }

                ParkingSpaces.AddRange(spaces);
                SaveChanges();
            }
        }
    }
}