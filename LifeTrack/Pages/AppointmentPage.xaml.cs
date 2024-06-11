using LifeTrack.Classes;
using LifeTrack.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LifeTrack.Pages
{
    public partial class AppointmentPage : Page
    {
        public AppointmentPage()
        {
            InitializeComponent();
            LoadDoctors();
            LoadTimes();
        }

        private void LoadDoctors()
        {
            var doctors = Connect.modeldb.users
                .Where(u => u.type_user.role == "therapist" || u.type_user.role == "traumatologist" || u.type_user.role == "orthodontist")
                .ToList();
            DoctorComboBox.ItemsSource = doctors;
            DoctorComboBox.DisplayMemberPath = "name";
            DoctorComboBox.SelectedValuePath = "id";
        }

        private void LoadTimes()
        {
            var times = Enumerable.Range(8, 9)
                .SelectMany(h => Enumerable.Range(0, 4)
                .Select(m => new TimeSpan(h, m * 15, 0)))
                .ToList();
            TimeComboBox.ItemsSource = times;
        }

        private void BookAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorComboBox.SelectedItem == null || DatePicker.SelectedDate == null || TimeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка при записи", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int doctorId = (int)DoctorComboBox.SelectedValue;
            DateTime appointmentDate = (DateTime)DatePicker.SelectedDate;
            TimeSpan appointmentTime = (TimeSpan)TimeComboBox.SelectedItem;

            var existingAppointment = Connect.modeldb.appointments
                .FirstOrDefault(a => a.doctor_id == doctorId && a.appointment_date == appointmentDate && a.appointment_time == appointmentTime);

            if (existingAppointment != null)
            {
                MessageBox.Show("Данное время уже занято. Пожалуйста, выберите другое время.", "Ошибка при записи", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newAppointment = new appointments
            {
                user_id = UchEntities.CurrentUser.id,
                doctor_id = doctorId,
                appointment_date = appointmentDate,
                appointment_time = appointmentTime
            };

            Connect.modeldb.appointments.Add(newAppointment);
            Connect.modeldb.SaveChanges();

            MessageBox.Show("Вы успешно записались на прием.", "Запись успешна", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
