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
                var user = db.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TBLogin.Text && u.Password == PasswordBox.Password);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким логином не найден!");
                    return;
                }
                else 
                {
                    MessageBox.Show("Пользователь успешно найден!");
                }

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
    }
}
