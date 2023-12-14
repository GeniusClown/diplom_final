using sportik.Classes;
using sportik.Models;
using sportik.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static sportik.Pages.Authorization;
using static sportik.Pages.plitka;

namespace sportik
{
    /// <summary>
    /// Логика взаимодействия для OneOrder.xaml
    /// </summary>
    public partial class OneOrder : Window
    {
        private ObservableCollection<OrderItem> orderItems;
        private int OrderItemCount => orderItems.Count;
        public OneOrder(ObservableCollection<OrderItem> orderItems)
        {
            InitializeComponent();
            this.orderItems = orderItems;
            OrderListView.ItemsSource = orderItems;
            UpdateTotalCost();
        }

        public OneOrder()
        {
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListView.SelectedItem != null)
            {
                OrderItem selectedOrderItem = OrderListView.SelectedItem as OrderItem;
                if (selectedOrderItem != null)
                {
                    orderItems.Remove(selectedOrderItem);
                    UpdateTotalCost();
                }
            }
        }

        private void UpdateTotalCost()
        {
            int totalCost = orderItems.Sum(item => (int)item.MerchPrice * item.Quantity);
            int totalDiscount = orderItems.Sum(item => item.Discount > 0 ? (int)item.MerchPrice * item.Quantity * item.Discount / 100 : 0);
            int totalCostWithDiscount = totalCost - totalDiscount;

            // Обновляем TextBlock с информацией о скидке и финальной цене
            DiscountInfoTextBlock.Text = $"Сумма заказа: {totalCost} руб.\nСкидка: {totalDiscount} руб.\nИтоговая цена: {totalCostWithDiscount} руб.";
        }

        public void SaveOrderToDatabase()
        {
            try
            {
                using (var context = new sportclubbd())
                {
                    var order = new orders
                    {
                        id_users = 3,
                        id_status = 1,
                        id_point = (OrderListView.Items[0] as OrderItem)?.SelectedPoint != null ?
                            int.Parse((OrderListView.Items[0] as OrderItem)?.SelectedPoint) :
                            1,
                        order_date = DateTime.Now,
                        code = GenerateOrderCode(),
                        cost = GetTotalCost(),
                        discount = GetTotalDiscount(),
                        total_cost = GetTotalCostWithDiscount(),

                        delivery_days = OrderItemCount > 3 ? 3 : 6 //Формула для сроков доставки
                    };
                    context.orders.Add(order);

                    foreach (var orderItem in orderItems)
                    {
                        var sostav = new sostav
                        {
                            id_merch = orderItem.MerchId,
                            count = orderItem.Quantity,
                            quantity = 1,
                            total_cost = (int)orderItem.MerchPrice * orderItem.Quantity,
                            discount = orderItem.Discount
                        };

                        order.sostav.Add(sostav);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении заказа в базу данных: {ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetTotalCost()
        {
            return orderItems.Sum(item => (int)item.MerchPrice * item.Quantity);
        }

        private int GetTotalDiscount()
        {
            return orderItems.Sum(item => item.Discount > 0 ? (int)item.MerchPrice * item.Quantity * item.Discount / 100 : 0);
        }

        private int GetTotalCostWithDiscount()
        {
            return GetTotalCost() - GetTotalDiscount();
        }

        private int GenerateOrderCode()
        {
            Random random = new Random();
            int number1 = random.Next(100, 1000);

            return number1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveOrderToDatabase();
            orderItems.Clear();
            Close();
            //MessageBox.Show("Заказ сохранен в базе данных!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}