using sportik.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sportik.Pages
{
    /// <summary>
    /// Логика взаимодействия для plitka.xaml
    /// </summary>
    public partial class plitka : Page
    {
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        private int _currentPage = 1, _countInPage = 3, _maxPages;
        public plitka()
        {
            InitializeComponent();
            var merchik = sportclubbd.GetContext().merch.ToList();
            LVMerch.ItemsSource = merchik;
            var allManuf = sportclubbd.GetContext().merch
                .Select(m => m.manufacturer)
                .Distinct()
                .ToList();

            allManuf.Insert(0, "Все производители");
            ComboMan.ItemsSource = allManuf;

            UpdateTours();
        }
        private void MerchBD_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (LVMerch.SelectedItem == null)
            {
                e.Handled = true;
            }
            else
            {
                // Получить выбранный элемент
                merch selectedMerch = LVMerch.SelectedItem as merch;

                // Вывести MessageBox с информацией о выбранном товаре
                MessageBox.Show($"Selected Merch:\nName: {selectedMerch.name}\nManufacturer: {selectedMerch.manufacturer}\nPrice: {selectedMerch.price}", "Merchandise Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public class OrderItem
        {
            public int OrderId { get; set; }
            public int UserId { get; set; }
            public int StatusId { get; set; }
            public int PointId { get; set; }
            public DateTime OrderDate { get; set; }
            public int Code { get; set; }
            public int Cost { get; set; }
            public int Discount { get; set; }
            public int MerchId { get; set; }
            public string MerchName { get; set; }
            public decimal MerchPrice { get; set; }
            public int Quantity { get; set; }
            public string SelectedPoint { get; set; }
            public int Point { get; set; }
        }
        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LVMerch.SelectedItem != null)
            {
                merch selectedMerch = LVMerch.SelectedItem as merch;

                OrderItem orderItem = new OrderItem
                {
                    MerchId = selectedMerch.id,
                    MerchName = selectedMerch.name,
                    MerchPrice = selectedMerch.price,
                    Quantity = 1
                };

                if (selectedMerch.discount.HasValue)
                {
                    orderItem.Discount = selectedMerch.discount.Value;
                }
                else
                {
                    MessageBox.Show("У товара нет скидки");
                }

                orderItems.Add(orderItem);

                UpdateOrderViewButtonVisibility();
            }
        }
        private int GenerateOrderCode()
        {
            Random random = new Random();
            int number1 = random.Next(100, 1000);

            return int.Parse($"{number1}");
        }
        private void UpdateOrderViewButtonVisibility()
        {
            if (orderItems.Any())
            {
                ShowOrderButton.Visibility = Visibility.Visible;
            }
            else
            {
                ShowOrderButton.Visibility = Visibility.Collapsed;
            }
        }
        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OneOrder orderViewWindow = new OneOrder(orderItems);
            orderViewWindow.ShowDialog();
        }

        public void UpdateTours() 
        {
            var currentMerch = sportclubbd.GetContext().merch.ToList();
            if (ComboMan.SelectedIndex > 0)
            {
                var selectedManufacturer = (ComboMan.SelectedItem as string);
                if (!string.IsNullOrEmpty(selectedManufacturer))
                {
                    currentMerch = currentMerch.Where(p => p.manufacturer.Contains(selectedManufacturer)).ToList();
                }
            }

            currentMerch = currentMerch.Where(p => p.name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (CheckDiscount.IsChecked == true)
            {
                currentMerch = currentMerch.Where(p => p.discount > 0).ToList();
            }
            LVMerch.ItemsSource = currentMerch;
            
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        private void ComboMan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }

        private void CheckDis(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        private void BtnFirstPage_Click(object sender, RoutedEventArgs e) => ChangePage(1);

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e) => ChangePage(_currentPage - 1);

        private void BtnNextPage_Click(object sender, RoutedEventArgs e) => ChangePage(_currentPage + 1);

        private void BtnLastPage_Click(object sender, RoutedEventArgs e) => ChangePage(_maxPages);

        private void ChangePage(int page)
        {
            _currentPage = Math.Max(1, Math.Min(page, _maxPages));
            RefreshData();
        }

        private void RefreshData()
        {
            var merchik = sportclubbd.GetContext().merch.ToList();
            _maxPages = (int)Math.Ceiling(merchik.Count * 1.0 / _countInPage);
            merchik = merchik.Skip((_currentPage - 1) * _countInPage).Take(_countInPage).ToList();

            LblPages.Content = $"{_currentPage}/{_maxPages}";
            LVMerch.ItemsSource = merchik;

            ManageButtonsEnable();
            GeneratePageNumbers();
        }

        private void GeneratePageNumbers()
        {
            SPanelPages.Children.Clear();

            foreach (int i in Enumerable.Range(1, _maxPages))
            {
                Button btn = new Button { Content = i.ToString(), Width = 28 };
                btn.Click += (sender, e) => ChangePage(int.Parse(((Button)sender).Content.ToString()));
                SPanelPages.Children.Add(btn);
            }
        }

        private void ManageButtonsEnable()
        {
            bool atFirstPage = _currentPage == 1;
            bool atLastPage = _currentPage == _maxPages;

            BtnFirstPage.IsEnabled = BtnPreviousPage.IsEnabled = !atFirstPage;
            BtnLastPage.IsEnabled = BtnNextPage.IsEnabled = !atLastPage;
        }
    }
}
