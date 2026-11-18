using RestaurantManager.Services;
using RestourantManager.Models;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManager.Views
{
    public partial class KitchenWindow : Window
    {
        public KitchenWindow()
        {
            InitializeComponent();
            LoadQueue();
        }

        private void LoadQueue()
        {
            LvKitchenItems.ItemsSource = OrderService.Instance.GetKitchenQueue();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadQueue();
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn && btn.Tag is KitchenDishItem item)
            {
                if (item.isReady)
                    return;

                OrderService.Instance.MarkDishAsReady(item.OrderId, item.OrderItemId);
                LoadQueue();
            }
        }

        private void BtnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
