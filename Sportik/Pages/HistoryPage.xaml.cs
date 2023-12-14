using sportik.Classes;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            LoadLoginHistory();
        }
        private void LoadLoginHistory()
        {
            try
            {
                // Получаем историю входов из базы данных
                var loginHistory = Connect.modeldb.loginhistory.ToList();

                // Отображаем историю входов на странице
                HistoryDataGrid.ItemsSource = loginHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке истории входов: " + ex.Message.ToString(),
                                "Критическая работа приложения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
