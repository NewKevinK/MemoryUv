using Data;
using Host;
using Logic;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public MemoryServer service;
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = Usuario.Text;
            string password = pbPassword.Password.ToString();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                service = new MemoryServer();
                UserGame user = service.GetLoggerUser(email, password);
                if (user != null)
                {
                    service.updateUserStatus(user.id, "Activo");
                    Home windowHome = new Home(user);
                    windowHome.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrecta, revisa bien!");
                }             
            }
            else
            {
                MessageBox.Show("Existe campo vacio");
            }
        }

        private void ButtonNewAccountClick(object sender, RoutedEventArgs e)
        {
            Register windowRegister = new Register();
            windowRegister.Show();
            this.Close();
        }

        private void SupportClick(object sender, RoutedEventArgs e)
        {
            ChangePassword windowChangePassword = new ChangePassword();
            windowChangePassword.Show();
            this.Close();
        }
    }
}
