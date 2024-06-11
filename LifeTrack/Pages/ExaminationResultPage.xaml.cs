using LifeTrack.Classes;
using LifeTrack.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LifeTrack.Pages
{
    public partial class ExaminationResultPage : Page
    {
        private int _appointmentId;

        public ExaminationResultPage(int appointmentId)
        {
            InitializeComponent();
            _appointmentId = appointmentId;
            LoadData();
        }

        private void LoadData()
        {
            var appointment = Connect.modeldb.appointments.FirstOrDefault(a => a.id == _appointmentId);
            if (appointment != null)
            {
                ExaminationResultTextBox.Text = appointment.examination_result;
                PrescriptionComboBox.Text = appointment.prescription;
                AdviceTextBox.Text = appointment.advice;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var appointment = Connect.modeldb.appointments.FirstOrDefault(a => a.id == _appointmentId);
            if (appointment != null)
            {
                appointment.examination_result = ExaminationResultTextBox.Text;
                appointment.prescription = ((ComboBoxItem)PrescriptionComboBox.SelectedItem)?.Content.ToString();
                appointment.advice = AdviceTextBox.Text;

                Connect.modeldb.SaveChanges();
                MessageBox.Show("Данные сохранены.");
                Manager.MainFrame.GoBack();
            }
        }
    }
}
