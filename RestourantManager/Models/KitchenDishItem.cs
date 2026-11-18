using System;
using System.Collections.Generic;
using System.Text;

namespace RestourantManager.Models
{
    // допоміжний клас
    public class KitchenDishItem
    {
        public int OrderId { get; set; }
        public int  TableId { get; set; }
        public int OrderItemId { get; set; }
        public string DishName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool isReady {  get; set; }
        public string StatusDisplay => isReady ? "Готово" : "В черзі";
    }
}
