using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;

namespace RestaurantManager.Data
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RestaurantManagerDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, Name = "Стіл 1", Seats = 2, IsOccupied = false },
                new Table { Id = 2, Name = "Стіл 2", Seats = 4, IsOccupied = true },
                new Table { Id = 3, Name = "Стіл 3", Seats = 4, IsOccupied = false },
                new Table { Id = 4, Name = "Стіл 4", Seats = 6, IsOccupied = false }
            );
        }
    }
}