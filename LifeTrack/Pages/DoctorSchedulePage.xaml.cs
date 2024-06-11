using LifeTrack.Classes;
using LifeTrack.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LifeTrack.Pages
{
    public partial class DoctorSchedulePage : Page
    {
        public DoctorSchedulePage()
        {
            InitializeComponent();
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            var schedule = from a in Connect.modeldb.appointments
                           join p in Connect.modeldb.pacient on a.user_id equals p.user_id
                           where a.doctor_id == UchEntities.CurrentUser.id
                           select new
                           {
                               AppointmentId = a.id, // Ensure this property is included
                               Name = p.name,
                               Age = p.old,
                               Gender = p.gender,
                               Phone = p.phone,
                               Disease = p.desease,
                               AppointmentDate = a.appointment_date,
                               AppointmentTime = a.appointment_time
                           };
            ScheduleDataGrid.ItemsSource = schedule.ToList();
        }

        private void ExaminationResult_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int appointmentId = (int)button.Tag;
                // Navigate to the examination result page
                Manager.MainFrame.Navigate(new ExaminationResultPage(appointmentId));
            }
        }


    }
}