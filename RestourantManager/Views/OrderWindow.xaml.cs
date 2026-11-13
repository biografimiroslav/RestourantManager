using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RestaurantManager.Models;
using RestaurantManager.Services;

namespace RestaurantManager.Views
{
    public partial class OrderWindow : Window
    {
        private readonly int _tableId;
        private readonly TableService _tableService = TableService.Instance;
        private readonly OrderService _orderService = OrderService.Instance;
        private Order? _currentOrder;
        private List<Dish> _allDishes = new();

        public OrderWindow(int tableId)
        {
            InitializeComponent();
            _tableId = tableId;
            Loaded += (s, e) => LoadData();
            Closing += (s, e) => OnWindowClosing();
        }

        private void LoadData()
        {
            var table = _tableService.GetTableById(_tableId);
            if (table == null)
            {
                MessageBox.Show("Стіл не знайдено!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            TxtTableTitle.Text = $"{table.Name} ({table.Seats} місць)";

            _currentOrder = _orderService.GetOrCreateActiveOrder(_tableId);

            _allDishes = _orderService.GetAllDishes();
            LvMenuDishes.ItemsSource = _allDishes;

            RefreshOrderItems();
        }

        private void RefreshOrderItems()
        {
            if (_currentOrder == null) return;

            var items = _currentOrder.Items.ToList();
            LvOrderItems.ItemsSource = null;
            LvOrderItems.ItemsSource = items;

            TxtTotalAmount.Text = $"{_currentOrder.TotalAmount:N0} ₴";

            bool hasItems = items.Count > 0;
            BtnPayAndClose.IsEnabled = hasItems;
            BtnPayAndClose.Opacity = hasItems ? 1.0 : 0.5;
        }

        private void BtnAddDish_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int dishId && _currentOrder != null)
            {
                var dish = _allDishes.FirstOrDefault(d => d.Id == dishId);
                if (dish != null)
                {
                    _orderService.AddDishToOrder(_currentOrder.Id, dish);
                    RefreshOrderItems();
                }
            }
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int dishId && _currentOrder != null)
            {
                var dish = _allDishes.FirstOrDefault(d => d.Id == dishId);
                if (dish != null)
                {
                    _orderService.AddDishToOrder(_currentOrder.Id, dish);
                    RefreshOrderItems();
                }
            }
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int orderItemId && _currentOrder != null)
            {
                _orderService.RemoveItemFromOrder(_currentOrder.Id, orderItemId);
                RefreshOrderItems();
            }
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnPayAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrder == null || _currentOrder.Items.Count == 0)
            {
                MessageBox.Show("Чек порожній! Спочатку додайте страви до замовлення.", "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Оплатити чек на суму {_currentOrder.TotalAmount:N0} ₴ та звільнити стіл?",
                "Оплата та закриття",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _orderService.CloseAndPayOrder(_currentOrder.Id);
                Close();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnWindowClosing()
        {
            _orderService.CleanEmptyOrders();
        }

        private void LvOrderItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
