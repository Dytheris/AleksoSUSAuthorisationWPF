using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        private readonly SUSEntities db;

        public Reg()
        {
            InitializeComponent();
            db = new SUSEntities();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginBox.Text = string.Empty;
            PassswordBox.Password = string.Empty;
            PassswordBoxAccept.Password = string.Empty;
            FIOBox.Text = string.Empty;
            Roles.SelectedIndex = 1;
            NavigationService.GoBack();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = GetHash(PassswordBox.Password);
            string passwordAccept = GetHash(PassswordBoxAccept.Password);
            string FIO = FIOBox.Text;
            string photo = UserPhoto.Text;
            int Role = (Roles.SelectedItem as ComboBoxItem)?.Tag as string == "1" ? 1 : 2;

            try
            {
                if (login.Length < 3)
                {
                    MessageBox.Show("Логин не может быть меньше 3 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (db.Users.Any(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LoginBox.Text = "";
                    return;
                }


                if (passwordAccept != password)
                {
                    MessageBox.Show("Пароль не совпадает с введённым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Roles.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newUser = new User
                {
                    Login = login,
                    Password = password,
                    FIO = FIO,
                    Photo = photo,
                    Role = Role
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Регистрация завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    foreach (var valError in entityErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Ошибка: {valError.ErrorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
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
    }
}