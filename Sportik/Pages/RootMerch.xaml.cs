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
    /// Логика взаимодействия для RootMerch.xaml
    /// </summary>
    public partial class RootMerch : Page
    {
        private ObservableCollection<merch> merchCollection;
        public RootMerch()
        {
            InitializeComponent();
            merchCollection = new ObservableCollection<merch>(sportclubbd.GetContext().merch.ToList());
            MerchBD.ItemsSource = merchCollection;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new EditMerch(null));
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

        private void Orders(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow(); ;
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Close();
            ordersWindow.Show();
        }

        private void RefreshPage()
        {
            merchCollection.Clear();
            foreach (var merch in sportclubbd.GetContext().merch.ToList())
            {
                merchCollection.Add(merch);
            }
        }

        private void Ref(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void History(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new HistoryPage());
        }
    }
}