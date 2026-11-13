using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManager.Models
{
    public enum OrderStatus
    {
        Active,
        Paid
    }

    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<OrderItem> Items { get; set; } = new();

        public decimal TotalAmount => Items?.Sum(i => i.TotalPrice) ?? 0;
        public string StatusDisplay => Status == OrderStatus.Active ? "Відкрито" : "Оплачено";
    }
}
