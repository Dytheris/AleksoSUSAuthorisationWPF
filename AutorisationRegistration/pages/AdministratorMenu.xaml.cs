using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace AutorisationRegistration.pages
{
    /// <summary>
    /// Логика взаимодействия для AdministratorMenu.xaml
    /// </summary>
    public partial class AdministratorMenu : Page
    {
        private readonly SUSEntities db;
        public AdministratorMenu()
        {
            InitializeComponent();
            db = new SUSEntities();

            DataGridUsers.ItemsSource = db.Users.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new pages.NewUsr());
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridUsers.SelectedItem != null)
            {
                var idselectedUser = (DataGridUsers.SelectedItems.Cast<User>().Select(u => u.ID).ToList());
                if (MessageBox.Show($"Вы точно хотите удалить", "Я тебя спрашиваю!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var idselectetuser = db.Users.Where(u => idselectedUser.Contains(u.ID)).ToList();
                        db.Users.RemoveRange(idselectetuser);
                        db.SaveChanges();
                        DataGridUsers.ItemsSource = db.Users.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new pages.NewUsr((sender as Button).DataContext as User));
        }
    }
}
