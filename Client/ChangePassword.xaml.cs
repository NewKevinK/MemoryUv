using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public MemoryServer service;
        string codex;
        UserGame user;
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            string code = tbxCode.Text;
            string password = pbPassword.Password.ToString();
            string passwordRepit = pbPasswordRepit.Password.ToString();
            if (!ExistsEmptyFields(code, password, passwordRepit) && !ExistsInvalidPassword(password, passwordRepit)){
                if (code.Equals(codex))
                {
                    SendToModify(password);
                }
                else
                {
                    MessageBox.Show("Codigo incorrecto");
                }
            }
        }

        private void SendCodeClick(object sender, RoutedEventArgs e)
        {
            if (!ExistsInvalidEmail(tbxEmail.Text))
            {
                codex = service.SendEmail(tbxEmail.Text);
                tbxCode.IsEnabled = true;
                MessageBox.Show("Codigo enviado...");
            }
        }

        private void SendToModify(string password)
        {
            service = new MemoryServer();
            bool updated = service.UpdatePassword(user.id, password);
            if (updated)
            {
                MessageBox.Show("Se actualizo correctamente la contraseña");
            }       
        }

        private bool ExistsEmptyFields(string code, string password, string passwordRepit)
        {
            bool exists = false;
            if (string.IsNullOrEmpty(tbxCode.Text) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordRepit))
            {
                MessageBox.Show("Existe campo vacío");
                exists = true;
            }
            return exists;
        }

        private bool ExistsInvalidEmail(string email)
        {
            bool exists = false;
            try
            {
                MailAddress mailAdress = new MailAddress(email);
            }
            catch (FormatException)
            {
                exists = true;
                MessageBox.Show("Existen caracteres invalidos en el correo electronico");
            }

            if (exists != true)
            {
                if (string.IsNullOrEmpty(email))
                {
                    exists = true;
                    MessageBox.Show("Existe campo vacío");
                }
            }
            
            if(exists != true)
            {
                service = new MemoryServer();
                UserGame user = service.GetUserByEmail(tbxEmail.Text);
                if(user != null)
                {
                    this.user = user;
                }
                else
                {
                    exists = true;
                    MessageBox.Show("No existe un usuario asociado al correo");
                }
            }
            return exists;
        }

        private bool ExistsInvalidPassword(string password, string passwordRepit)
        {
            bool exists = false;
            if (!ExistsInvalidCharacters(password))
            {
                if (!password.Equals(passwordRepit))
                {
                    exists = true;
                    MessageBox.Show("Las contraseñas no son iguales");
                }
            }
            else
            {
                exists = true;
            }
            return exists;
        }

        private bool ExistsInvalidCharacters(string password)
        {
            bool exists = false;
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{8,15}$");
            if (!regex.IsMatch(password))
            {
                exists = true;
                MessageBox.Show("Contraseña insegura \n"
                    + "La contraseña debe tener entre 8 y 16 caracteres \n"
                    + "La contraseña debe tener por lo menos un digito \n"
                    + "La contraseña debe tener por lo menos una letra mayúscula \n"
                    + "La contraseña debe tener por lo menos una letra minúscula");
            }
            return exists;
        }
    }
}
