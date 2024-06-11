
using LifeTrack.Pages;
using LifeTrack.Classes;
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
using System.Windows.Threading;

namespace LifeTrack
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Info());
            Manager.MainFrame = MainFrame;
        }
        private void Autorization(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Authorization());
        }  
        

        private void root(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pacients());
        }
      

        private void Regestration(object sender, RoutedEventArgs e)
        {

            Manager.MainFrame.Navigate(new Registration());

        }

        private void Info(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Info());
        }        
        private void Exit(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void Spravka(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Spravka());
        }
    }
}
