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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Логин и пароль не введены,\n либо введены неправильно");
                return;
            }

            using (var db = new RegistrationAutorisationEntities())
            {
                string login = TBLogin.Text;
                string hashedPassword = GetHash(PasswordBox.Password);

                if (TBLogin.Text.Length < 3)
                {
                    MessageBox.Show("Логин должен иметь не меньше 5 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (PasswordBox.Password.Length < 3)
                {
                    MessageBox.Show("Пароль должен иметь не меньше 5 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == login && u.Password == hashedPassword);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Вы авторизовались!");

                switch (user.Role)
                {
                    case 1:
                        NavigationService?.Navigate(new AdministratorMenu());
                        break;
                    case 2:
                        NavigationService?.Navigate(new UserMenu());
                        break;
                }
            }
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Reg());
        }
    }
}
