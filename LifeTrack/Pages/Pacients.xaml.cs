using LifeTrack.Classes;
using LifeTrack.Models;
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

namespace LifeTrack.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pacients.xaml
    /// </summary>
    public partial class Pacients : Page
    {
        private ObservableCollection<pacient> merchCollection;
        public Pacients()
        {
            InitializeComponent();
            merchCollection = new ObservableCollection<pacient>(UchEntities.GetContext().pacient.ToList());
            MerchBD.ItemsSource = merchCollection;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            // Pass the current user's ID when adding a new patient
            Manager.MainFrame.Navigate(new EditPacient(null, UchEntities.CurrentUser.id));
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            // Pass the current user's ID when editing a patient
            Manager.MainFrame.Navigate(new EditPacient((sender as Button).DataContext as pacient, UchEntities.CurrentUser.id));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var MerchDell = MerchBD.SelectedItems.Cast<pacient>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {MerchDell.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    UchEntities.GetContext().pacient.RemoveRange(MerchDell);
                    UchEntities.GetContext().SaveChanges();
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


        private void RefreshPage()
        {
            merchCollection.Clear();
            foreach (var merch in UchEntities.GetContext().pacient.ToList())
            {
                merchCollection.Add(merch);
            }
        }

        private void Ref(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void zapisivxod(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Zapisi(null));
        }
    }
}
