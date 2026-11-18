using RestourantManager.Models;
using RestourantManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace RestaurantManager.Views
{
    public partial class AdminWindow : Window
    {
        private ObservableCollection<Dish> _dishes;
        public AdminWindow()
        {
            InitializeComponent();
            _dishes = new ObservableCollection<Dish>(DishService.instance.GetAllDishes());
            DishesGrid.ItemsSource = _dishes;
        }

        // додавання
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string category = CategoryBox.Text.Trim();

            if(string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введіть назву страви!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(decimal.TryParse(PriceBox.Text.Replace('.','.'), out decimal price) || price <= 0)
            {
                MessageBox.Show("Введіть коректну ціну (число більше 0)!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newDish = DishService.instance.AddDish(name, price, string.IsNullOrEmpty(category) ? "Інше" : category);
            _dishes.Add(newDish);

            NameBox.Clear();
            PriceBox.Clear();
        }

        // вилучення
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(DishesGrid.SelectedItem is  Dish selectDish)
            {
                var result = MessageBox.Show($"Видалити страву «{selectDish.Name}»?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    DishService.instance.DeleteDish(selectDish.Id);
                    _dishes.Remove(selectDish);
                }
            }
            else
            {
                MessageBox.Show("Оберіть страву в таблиці для видалення.", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Автоматичне збереження зміни у файл
        private void DishesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(() => DishService.instance.SaveDishes());
        }
    }

}
