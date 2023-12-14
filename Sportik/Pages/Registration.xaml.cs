using sportik.Classes;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace sportik.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            Connect.modeldb = new Models.sportclubbd();
        }

        private void Registraciya_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей ввода
            string login = Login.Text;
            string password = Password.Password;
            string confirmPassword = ConfirmPassword.Password;

            // Проверяем, совпадают ли пароль и подтверждение пароля
            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка при регистрации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, существует ли пользователь с таким логином
            var existingUser = Connect.modeldb.users.FirstOrDefault(u => u.login == login);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка при регистрации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем нового пользователя
            var newUser = new Models.users
            {
                login = login,
                password = password,
                id_type = 3 // 3 соответствует роли "Client"
            };

            // Добавляем пользователя в таблицу users
            Connect.modeldb.users.Add(newUser);

            try
            {
                // Сохраняем изменения в базе данных
                Connect.modeldb.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Успешная регистрация",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Очищаем поля ввода
                Login.Clear();
                Password.Clear();
                ConfirmPassword.Clear();
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
                MessageBox.Show("Ошибка при регистрации: " + sb.ToString(), "Ошибка при регистрации",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
