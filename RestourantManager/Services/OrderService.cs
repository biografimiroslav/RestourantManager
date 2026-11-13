using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Services
{
    public class OrderService
    {
        public static OrderService Instance { get; } = new();

        private readonly List<Order> _orders = new();
        private int _nextOrderId = 1;

        public Order? GetActiveOrderByTableId(int tableId)
        {
            return _orders.FirstOrDefault(o => o.TableId == tableId && o.Status == OrderStatus.Active);
        }

        public Order GetOrCreateActiveOrder(int tableId)
        {
            var existing = GetActiveOrderByTableId(tableId);
            if (existing != null)
                return existing;

            var newOrder = new Order
            {
                Id = _nextOrderId++,
                TableId = tableId,
                Status = OrderStatus.Active,
                CreatedAt = DateTime.Now,
                Items = new List<OrderItem>()
            };
            _orders.Add(newOrder);
            return newOrder;
        }

        public List<Dish> GetAllDishes()
        {
            return new List<Dish>
            {
                new Dish { Id = 1, Name = "Борщ український", Price = 140, CategoryName = "Перші страви" },
                new Dish { Id = 2, Name = "Вареники з картоплею", Price = 110, CategoryName = "Гарячі страви" },
                new Dish { Id = 3, Name = "Деруни зі сметаною", Price = 120, CategoryName = "Гарячі страви" },
                new Dish { Id = 4, Name = "Салат Цезар", Price = 180, CategoryName = "Салати" },
                new Dish { Id = 5, Name = "Стейк з лосося", Price = 320, CategoryName = "Гарячі страви" },
                new Dish { Id = 6, Name = "Капучино", Price = 60, CategoryName = "Напої" },
                new Dish { Id = 7, Name = "Узвар", Price = 35, CategoryName = "Напої" },
                new Dish { Id = 8, Name = "Чізкейк", Price = 115, CategoryName = "Десерти" }
            };
        }

        public void AddDishToOrder(int orderId, Dish dish)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            var item = order.Items.FirstOrDefault(i => i.DishId == dish.Id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                order.Items.Add(new OrderItem
                {
                    Id = (order.Items.Count > 0 ? order.Items.Max(x => x.Id) : 0) + 1,
                    OrderId = order.Id,
                    DishId = dish.Id,
                    DishName = dish.Name,
                    UnitPrice = dish.Price,
                    Quantity = 1
                });
            }

            SyncTableStatus(order.TableId);
        }

        public void RemoveItemFromOrder(int orderId, int orderItemId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            var item = order.Items.FirstOrDefault(i => i.Id == orderItemId || i.DishId == orderItemId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    order.Items.Remove(item);
                }
            }

            SyncTableStatus(order.TableId);
        }

        public void CloseAndPayOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Paid;
                SyncTableStatus(order.TableId);
            }
        }

        public void CleanEmptyOrders()
        {
            var emptyOrders = _orders.Where(o => o.Status == OrderStatus.Active && (!o.Items.Any())).ToList();
            foreach (var o in emptyOrders)
            {
                _orders.Remove(o);
                SyncTableStatus(o.TableId);
            }
        }

        public void SyncTableStatus(int tableId)
        {
            var activeOrder = GetActiveOrderByTableId(tableId);
            bool isOccupied = activeOrder != null && activeOrder.Items.Any();

            try
            {
                using var db = new RestaurantDbContext();
                var table = db.Tables.FirstOrDefault(t => t.Id == tableId);
                if (table != null)
                {
                    table.IsOccupied = isOccupied;
                    db.SaveChanges();
                }
            }
            catch
            {
                
            }
        }
    }
}
