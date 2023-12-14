using sportik.Classes;
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
    /// Логика взаимодействия для merches.xaml
    /// </summary>
    public partial class merches : Page
    {
        private ObservableCollection<merch> merchCollection;
        public merches(users user)
        {
            InitializeComponent();
            merchCollection = new ObservableCollection<merch>(sportclubbd.GetContext().merch.ToList());
            MerchBD.ItemsSource = merchCollection;
            DataContext = user;
            SetVisibility(CurrentUser.UserRole);
        }

        private void SetVisibility(string role)
        {
            if (role == "Администратор")
            {
                RootContextMenu.Visibility = Visibility.Visible;
                ClientContextMenu.Visibility = Visibility.Collapsed;
                //RootPanel.Visibility = Visibility.Visible;
                ClientPanel.Visibility = Visibility.Collapsed;
            }
            else if (role == "Клиент")
            {
                ClientPanel.Visibility = Visibility.Visible;
                //RootPanel.Visibility = Visibility.Collapsed;
                RootContextMenu.Visibility = Visibility.Collapsed;
                ClientContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                ClientPanel.Visibility = Visibility.Collapsed;
                //RootPanel.Visibility = Visibility.Collapsed;
                RootContextMenu.Visibility = Visibility.Collapsed;
                ClientContextMenu.Visibility = Visibility.Visible;
            }
        }

        private void OneOrder(object sender, RoutedEventArgs e)
        {
            OneOrder oneOrder = new OneOrder(null);
            oneOrder.Show();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new EditMerch(null));
        }

        private void Orders(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow();
            this.Visibility = Visibility.Hidden;
            ordersWindow.Show();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new EditMerch((sender as Button).DataContext as merch));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var MerchDell = MerchBD.SelectedItems.Cast<merch>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {MerchDell.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    sportclubbd.GetContext().merch.RemoveRange(MerchDell);
                    sportclubbd.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    // Обновление ObservableCollection, что автоматически обновит DataGrid
                    foreach (var merch in MerchDell)
                    {
                        merchCollection.Remove(merch);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new EditMerch(null));
        }

        private void OrdersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow();
            this.Visibility = Visibility.Hidden;
            ordersWindow.Show();
        }
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MerchBD.SelectedItem != null)
            {
                Manager.MainFrame.Navigate(new EditMerch(MerchBD.SelectedItem as merch));
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedMerch = MerchBD.SelectedItem as merch;
            if (selectedMerch != null)
            {
                if (MessageBox.Show($"Вы точно хотите удалить выбранный товар?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        sportclubbd.GetContext().merch.Remove(selectedMerch);
                        sportclubbd.GetContext().SaveChanges();
                        MessageBox.Show("Товар удален!");

                        // Обновление ObservableCollection, что автоматически обновит DataGrid
                        merchCollection.Remove(selectedMerch);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void RefreshPage()
        {
            merchCollection.Clear();
            foreach (var merch in sportclubbd.GetContext().merch.ToList())
            {
                merchCollection.Add(merch);
            }
        }
        private void RefMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
