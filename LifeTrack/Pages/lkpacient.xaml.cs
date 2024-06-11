using LifeTrack.Classes;
using LifeTrack.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LifeTrack.Pages
{
    public partial class lkpacient : Page
    {
        public lkpacient()
        {
            InitializeComponent();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            var appointments = from a in Connect.modeldb.appointments
                               join d in Connect.modeldb.users on a.doctor_id equals d.id
                               join r in Connect.modeldb.type_user on d.id_type equals r.id
                               where a.user_id == UchEntities.CurrentUser.id
                               select new
                               {
                                   DoctorName = d.name,
                                   Specialization = r.role,
                                   AppointmentDate = a.appointment_date,
                                   AppointmentTime = a.appointment_time,
                                   ExaminationResult = a.examination_result,
                                   Prescription = a.prescription,
                                   Advice = a.advice
                               };

            var appointmentList = appointments.ToList();
            AppointmentDataGrid.ItemsSource = appointmentList;

            // Calculate completed and incomplete appointments
            int completedCount = appointmentList.Count(a => !string.IsNullOrEmpty(a.ExaminationResult));
            int incompleteCount = appointmentList.Count(a => string.IsNullOrEmpty(a.ExaminationResult));

            // Update the counters
            CompletedCount.Text = completedCount.ToString();
            IncompleteCount.Text = incompleteCount.ToString();
        }

        private void OpenAppointmentPage_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AppointmentPage());
        }
    }
}
