using AutorisationRegistration.pages;
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

namespace AutorisationRegistration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            UpdateCurrentTimeAndDate();
            MainFrame.Navigate(new Auth());
        }

        private void UpdateCurrentTimeAndDate()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdCurrentTimeAndDate;
            timer.Start();

        }

        private void UpdCurrentTimeAndDate(object sender, EventArgs e)
        {
            CurrentTimeAndDate.Text = DateTime.Now.ToString("G");
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page)) return;
            this.Title = $"ProjectBySUS - {page.Title}";

            if (page is pages.Auth)
                ButtonBack.Visibility = Visibility.Hidden;
            else
                ButtonBack.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из приложения?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }


        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }
    }
}
