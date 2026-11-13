using System.Collections.Generic;
using System.Linq;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Services
{
    public class TableService
    {
        public static TableService Instance { get; } = new();

        public List<Table> GetAllTables()
        {
            List<Table> tables;
            try
            {
                using var db = new RestaurantDbContext();
                db.Database.EnsureCreated();
                tables = db.Tables.ToList();
            }
            catch
            {
                // Fallback у разі тимчасової відсутності зв'язку з LocalDB
                tables = new List<Table>
                {
                    new Table { Id = 1, Name = "Стіл №1", Seats = 2, IsOccupied = false },
                    new Table { Id = 2, Name = "Стіл №2", Seats = 4, IsOccupied = false },
                    new Table { Id = 3, Name = "Стіл №3", Seats = 4, IsOccupied = false },
                    new Table { Id = 4, Name = "Стіл №4", Seats = 6, IsOccupied = false }
                };
            }

            // Статус столу строго синхронізується з наявністю страв у активному чеку:
            // Є страви в активному замовленні -> червоний (зайнятий)
            // Немає страв або замовлення оплачено -> зелений (вільний)
            foreach (var table in tables)
            {
                var activeOrder = OrderService.Instance.GetActiveOrderByTableId(table.Id);
                table.IsOccupied = activeOrder != null && activeOrder.Items.Count > 0;
            }

            return tables;
        }

        public Table? GetTableById(int tableId)
        {
            return GetAllTables().FirstOrDefault(t => t.Id == tableId);
        }

        public void UpdateTable(int tableId, TableStatus status, int orderId = 0)
        {
            try
            {
                using var db = new RestaurantDbContext();
                var table = db.Tables.FirstOrDefault(t => t.Id == tableId);
                if (table != null)
                {
                    table.IsOccupied = (status == TableStatus.Occupied);
                    db.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void ToggleTableStatus(int tableId)
        {
            try
            {
                using var db = new RestaurantDbContext();
                var table = db.Tables.FirstOrDefault(t => t.Id == tableId);
                if (table != null)
                {
                    table.IsOccupied = !table.IsOccupied;
                    db.SaveChanges();
                }
            }
            catch
            {
            }
        }
    }
}
