using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestourantManager.Services
{
    public class DishService
    {
        public static DishService instance { get; } = new();

        public List<Dish> GetAllDishes()
        {
            using var db = new RestaurantDbContext();
            db.Database.EnsureCreated();
            return db.Dishes.ToList();
        }

        public void SaveDishes()
        {
            using var db = new RestaurantDbContext();
            foreach (var dish in db.Dishes)
            {
                db.Entry(dish).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            db.SaveChanges();
        }

        public Dish AddDish(string name, decimal price, string categoryName = "Основне меню")
        {
            using var db = new RestaurantDbContext();
            var newDish = new Dish
            {
                Name = name,
                Price = price,
                CategoryName = categoryName
            };
            db.Dishes.Add(newDish);
            db.SaveChanges();
            return newDish;
        }

        public void DeleteDish(int id)
        {
            using var db = new RestaurantDbContext();
            var dish = db.Dishes.FirstOrDefault(d => d.Id == id);
            if(dish != null)
            {
                db.Dishes.Remove(dish);
                db.SaveChanges();
            }
        }
    }
}
