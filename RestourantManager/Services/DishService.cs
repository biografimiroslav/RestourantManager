using RestaurantManager.Data;
using RestaurantManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestourantManager.Services
{
    public class DishService
    {
        public static DishService instance { get; } = new();

        private readonly List<Dish> _inMemoryDishes = new();

        private int _nextId = 1;
        private bool _isDbLoaded = false;

        public List<Dish> GetAllDishes()
        {
            try
            {
                using var db = new RestaurantDbContext();
                db.Database.EnsureCreated();
                var dbDishes = db.Dishes.ToList();

                if (!_isDbLoaded)
                {
                    _inMemoryDishes.Clear();
                    _inMemoryDishes.AddRange(dbDishes);
                    _isDbLoaded = true;
                }
                return dbDishes;
            }
            catch
            {
                return _inMemoryDishes.ToList();
            }
        }

        public void SaveDishes()
        {
            try
            {
                using var db = new RestaurantDbContext();
                foreach (var inMemDish in _inMemoryDishes)
                {
                    var dbDish = db.Dishes.FirstOrDefault(d => d.Id == inMemDish.Id);
                    if (dbDish != null)
                    {
                        dbDish.Name = inMemDish.Name;
                        dbDish.Price = inMemDish.Price;
                        dbDish.CategoryName = inMemDish.CategoryName;
                    }
                }
                db.SaveChanges();
            }
            catch
            {
            }
        }

        public Dish AddDish(string name, decimal price, string categoryName = "Основне меню")
        {
            var newDish = new Dish
            {
                Id = _nextId++,
                Name = name,
                Price = price,
                CategoryName = categoryName
            };
            _inMemoryDishes.Add(newDish);

            try
            {
                using var db = new RestaurantDbContext();
                var dbDish = new Dish
                {
                    Name = name,
                    Price = price,
                    CategoryName = categoryName
                };
                db.Dishes.Add(dbDish);
                db.SaveChanges();
                newDish.Id = dbDish.Id;
            }
            catch
            {
            }

            return newDish;
        }

        public void DeleteDish(int id)
        {
            var inMem = _inMemoryDishes.FirstOrDefault(d => d.Id == id);
            if (inMem != null)
            {
                _inMemoryDishes.Remove(inMem);
            }

            try
            {
                using var db = new RestaurantDbContext();
                var dish = db.Dishes.FirstOrDefault(d => d.Id == id);
                if (dish != null)
                {
                    db.Dishes.Remove(dish);
                    db.SaveChanges();
                }
            }
            catch
            {
            }
        }
    }
}
