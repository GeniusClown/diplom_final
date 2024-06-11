using LifeTrack.Classes;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LifeTrack.Models;

namespace LifeTrack.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            Connect.modeldb = new UchEntities();
        }

        private void Registraciya_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей ввода
            string login = Login.Text;
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите роль!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int roleId = Convert.ToInt32((RoleComboBox.SelectedItem as ComboBoxItem)?.Tag);
            string password = Password.Password;
            string confirmPassword = ConfirmPassword.Password;

            // Проверяем, совпадают ли пароль и подтверждение пароля
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, существует ли пользователь с таким логином
            var existingUser = Connect.modeldb.users.FirstOrDefault(u => u.login == login);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, если выбрана роль администратора, отобразим поле для ввода кода
            if (roleId == 1)
            {
                AdminCodeTextBox.Visibility = Visibility.Visible;
                // Проверяем, если код администратора не введен или не соответствует "склифосовский", выходим из метода
                if (AdminCodeTextBox.Text.Trim() != "склифосовский")
                {
                    MessageBox.Show("Неверный код администратора!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Проверяем, если выбрана роль врача, проверим поле для ввода имени врача
            string doctorName = null;
            if (roleId == 1002 || roleId == 1003 || roleId == 1004)
            {
                doctorName = DoctorNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(doctorName))
                {
                    MessageBox.Show("Имя врача не может быть пустым!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Создаем нового пользователя
            var newUser = new users
            {
                login = login,
                password = password,
                id_type = roleId,
                name = doctorName // Указываем имя врача, если это врач
            };

            // Добавляем пользователя в таблицу users
            Connect.modeldb.users.Add(newUser);

            try
            {
                // Сохраняем изменения в базе данных
                Connect.modeldb.SaveChanges();

                // Устанавливаем текущего пользователя
                UchEntities.CurrentUser = newUser;

                MessageBox.Show("Регистрация прошла успешно!", "Успешная регистрация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Перенаправляем пользователя на страницу в зависимости от его роли
                switch (roleId)
                {
                    case 1:
                        // Администратор
                        Manager.MainFrame.Navigate(new Pacients());
                        break;
                    case 3:
                        // Клиент
                        Manager.MainFrame.Navigate(new EditPacient(null, newUser.id));
                        break;
                    case 1002:
                    case 1003:
                    case 1004:
                        // Врач (Терапевт, Травматолог, Ортодонт)
                        Manager.MainFrame.Navigate(new DoctorSchedulePage());
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя!", "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                MessageBox.Show("Ошибка при регистрации: " + sb.ToString(), "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка при регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoleComboBox.SelectedItem != null)
            {
                int roleId = Convert.ToInt32((RoleComboBox.SelectedItem as ComboBoxItem)?.Tag);
                if (roleId == 1 || roleId == 1002 || roleId == 1003 || roleId == 1004)
                {
                    AdminCodeTextBox.Visibility = Visibility.Visible;
                    AdminCodeHintTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    AdminCodeTextBox.Visibility = Visibility.Collapsed;
                    AdminCodeHintTextBlock.Visibility = Visibility.Collapsed;
                }

                if (roleId == 1002 || roleId == 1003 || roleId == 1004)
                {
                    DoctorNameTextBox.Visibility = Visibility.Visible;
                    DoctorNameHintTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    DoctorNameTextBox.Visibility = Visibility.Collapsed;
                    DoctorNameHintTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
