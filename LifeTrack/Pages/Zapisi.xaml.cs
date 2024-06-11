using LifeTrack.Classes;
using LifeTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
/// <summary>
/// Логика взаимодействия для Zapisi.xaml
/// </summary>
namespace LifeTrack.Pages
{
   
    public partial class Zapisi : Page
    {
        private orders currentorder = new orders();
        public Zapisi(orders sellectedOrder)
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            string[] propertiesToCheck = { currentorder.id_users.ToString(), currentorder.id_status.ToString(), currentorder.id_point.ToString() };

            string[] propertyNames = { "Укажите клиента", "Укажите статус", "Укажите пункт выдачи" };

            for (int i = 0; i < propertiesToCheck.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(propertiesToCheck[i]))
                {
                    errors.AppendLine(propertyNames[i]);
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (currentorder.id == 0)
                {
                    UchEntities.GetContext().orders.Add(currentorder);
                }

                using (DbContextTransaction dbContextTransaction = UchEntities.GetContext().Database.BeginTransaction())
                {
                    UchEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    dbContextTransaction.Commit();
                }

                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
