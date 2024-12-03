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

namespace AutorisationRegistration.pages
{
    /// <summary>
    /// Логика взаимодействия для UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Page
    {
        private SUSEntities db;
        public UserMenu()
        {
            InitializeComponent();
            db = new SUSEntities();
            ListUser.ItemsSource = db.Users.ToList();
            UpdateUsers();
        }

        private void FindUserFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }
        private void UpdateUsers()
        {
            var _currentUsers = db.Users.AsQueryable();


            if (!string.IsNullOrWhiteSpace(FindUserFIO.Text))
            {
                _currentUsers = _currentUsers
                    .Where(x => x.FIO != null && x.FIO.ToLower().Contains(FindUserFIO.Text.ToLower()));
            }


            if (OnlyUsers.IsChecked == true)
            {
                _currentUsers = _currentUsers.Where(x => x.Role1.Role1 == "User");
            }


            if (Filter.SelectedIndex == 0)
            {
                _currentUsers = _currentUsers.OrderBy(x => x.FIO);
            }
            else if (Filter.SelectedIndex == 1)
            {
                _currentUsers = _currentUsers.OrderByDescending(x => x.FIO);
            }


            ListUser.ItemsSource = _currentUsers.ToList();
        }

        private void OnlyUsers_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void OnlyUsers_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void BtnFilterClear_Click(object sender, RoutedEventArgs e)
        {
            FindUserFIO.Text = "";
            OnlyUsers.IsChecked = false;
            Filter.SelectedIndex = 0;
            UpdateUsers();

        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }
    }
}
