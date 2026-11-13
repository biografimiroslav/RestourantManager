using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RestaurantManager.Models;
using RestaurantManager.Services;

namespace RestaurantManager.Views
{
    public partial class MainWindow : Window
    {
        private readonly TableService _tableService = TableService.Instance;

        public MainWindow()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            TablesPanel.Children.Clear();
            var tables = _tableService.GetAllTables();

            foreach (var table in tables)
            {
                var btn = new Button
                {
                    Width = 170,
                    Height = 120,
                    Margin = new Thickness(12),
                    Tag = table,
                    Foreground = Brushes.White,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    BorderThickness = new Thickness(0),
                    Background = table.IsOccupied
                        ? new SolidColorBrush(Color.FromRgb(210, 65, 80))   // Червоний — зайнятий
                        : new SolidColorBrush(Color.FromRgb(55, 165, 105)), // Зелений — вільний
                    Content = $"{table.Name}\n({table.Seats} місць)\n[{(table.IsOccupied ? "Зайнятий" : "Вільний")}]"
                };

                btn.Click += TableButton_Click;
                TablesPanel.Children.Add(btn);
            }
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Table selectedTable)
            {
                // Передаємо Id обраного столу у вікно замовлення
                var orderWindow = new OrderWindow(selectedTable.Id);
                orderWindow.Title = $"Замовлення — {selectedTable.Name} ({(selectedTable.IsOccupied ? "Активний чек" : "Нове замовлення")})";
                orderWindow.ShowDialog();

                LoadTables();
            }
        }

        private void OpenKitchen_Click(object sender, RoutedEventArgs e)
        {
            new KitchenWindow().Show();
        }

        private void OpenAdmin_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }
    }
}
