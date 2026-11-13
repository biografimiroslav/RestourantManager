namespace RestaurantManager.Models
{
    public enum TableStatus
    {
        Free,
        Occupied
    }

    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public int Seats { get; set; }                   // Кількість місць
        public bool IsOccupied { get; set; }             // true - зайнятий, false - вільний

        public int TableNumber => Id;
        public int Capacity => Seats;
        public string StatusDisplay => IsOccupied ? "Зайнятий" : "Вільний";
        public TableStatus Status => IsOccupied ? TableStatus.Occupied : TableStatus.Free;
    }
}
