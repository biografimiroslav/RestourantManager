using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;

namespace RestaurantManager.Data
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Dish> Dishes { get; set; }

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

            modelBuilder.Entity<Dish>().HasData(
                new Dish { Id = 1, Name = "Борщ український", Price = 140, CategoryName = "Перші страви" },
                new Dish { Id = 2, Name = "Вареники з картоплею", Price = 110, CategoryName = "Гарячі страви" },
                new Dish { Id = 3, Name = "Деруни зі сметаною", Price = 120, CategoryName = "Гарячі страви" },
                new Dish { Id = 4, Name = "Салат Цезар", Price = 180, CategoryName = "Салати" },
                new Dish { Id = 5, Name = "Стейк з лосося", Price = 320, CategoryName = "Гарячі страви" },
                new Dish { Id = 6, Name = "Капучино", Price = 60, CategoryName = "Напої" },
                new Dish { Id = 7, Name = "Узвар", Price = 35, CategoryName = "Напої" },
                new Dish { Id = 8, Name = "Чізкейк", Price = 115, CategoryName = "Десерти" }
            );
        }
    }
}