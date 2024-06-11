using LifeTrack.Classes;
using LifeTrack.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditPacient.xaml
    /// </summary>
    public partial class EditPacient : Page
    {
        public OpenFileDialog ofd = new OpenFileDialog();
        private string newsourthpath = string.Empty;
        private bool flag = false;
        private pacient currentmerch = new pacient();
        private int userId;

        public EditPacient(pacient sellectedMerch, int userId)
        {
            InitializeComponent();
            this.userId = userId;
            if (sellectedMerch != null)
            {
                currentmerch = sellectedMerch;
            }
            DataContext = currentmerch;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            string[] propertiesToCheck = { currentmerch.name, currentmerch.gender, currentmerch.phone };

            string discountText = currentmerch.desease?.ToString() ?? string.Empty;
            string[] propertyNames = { "Имя", "Пол", "Телефон" };

            // Добавим регулярное выражение для проверки формата номера телефона
            string phoneRegex = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";

            for (int i = 0; i < propertiesToCheck.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(propertiesToCheck[i]))
                {
                    errors.AppendLine(propertyNames[i]);
                }
            }

            // Проверка на корректное значение скидки
            if (!string.IsNullOrWhiteSpace(discountText) && (!int.TryParse(discountText, out int discountValue) || discountValue < 0 || discountValue > 99))
            {
                errors.AppendLine("Скидка должна быть числом от 0 до 99.");
            }
            // Проверим формат номера телефона
            if (!Regex.IsMatch(currentmerch.phone, phoneRegex))
            {
                errors.AppendLine("Некорректный формат номера телефона");
            }
            if (currentmerch.old == 0)
            {
                errors.AppendLine("Укажите возраст пациента");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (currentmerch.id == 0)
                {
                    currentmerch.user_id = this.userId; // Ensure the user_id is set
                    UchEntities.GetContext().pacient.Add(currentmerch);
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

        private void Foto(object sender, RoutedEventArgs e)
        {
            string source = Environment.CurrentDirectory;
            if (ofd.ShowDialog() == true)
            {
                flag = true;
                string sourthpath = ofd.SafeFileName;
                newsourthpath = System.IO.Path.Combine(source.Replace("/bin/Debug", "/photo/"), sourthpath);

                // Проверка на null перед установкой изображения
                if (ofd.FileName != null)
                {
                    PreviewImage.Source = new BitmapImage(new Uri(ofd.FileName));
                }

                currentmerch.photo = $"/photo/{ofd.SafeFileName}";
            }
        }
    }
}
