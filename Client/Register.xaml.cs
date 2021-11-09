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
using Data;
using Logic;
using Host;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Registrar.xaml
    /// </summary>
    public partial class Register : Window
    {
        public MemoryServer service;
        string codex;
        

        public Register()
        {
            InitializeComponent();
        }

        private void ButtonSalir_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void ButtonContinuar_Click(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();            
            string email = tbCorreo.Text;
            string password = pbPassword.Password.ToString();
            
             
            if (!ExistsInvalidFields(email, password))
            {
                MessageBox.Show("Codigo enviado...");
                codex = service.SendEmail(email);
                tbCodigo.IsEnabled = true;
                tbNametag.IsEnabled = true;
                
            }
        }

        private void ButtonAceptar_Click(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();
            string code = tbCodigo.Text;
            string nametag = tbNametag.Text;
            string email = tbCorreo.Text;
            string password = pbPassword.Password.ToString();

            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(nametag) && !ExistsInvalidNametag(nametag))
            {
                if(code == codex)
                {
                    bool saved = service.RegisterUser(email, password, nametag);
                    if (saved)
                    {
                        MessageBox.Show("Se agrego correctamente al usuario");
                    }
                }
                else
                {
                    MessageBox.Show("Codigo incorrecto");
                }
                

                Login login = new Login();
                login.Show();
                this.Hide();
                
                
            }
            else
            {
                MessageBox.Show("Existe un campo vacio!");
            }
        }

        private Boolean ExistsInvalidFields(string email, string password)
        {
            bool exists = false;
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                exists = true;
                MessageBox.Show("Existe un campo vacio!");
            }else if (ExistsInvalidEmail(email) || ExistsInvalidPassword(password) || ExistsEqualEmail(email))
            {
                exists = true;
            }
            return exists;
        }

        private Boolean ExistsInvalidEmail(string email)
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
            return exists;
        }

        private Boolean ExistsInvalidPassword(string password)
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

        private Boolean ExistsInvalidNametag(string nametag)
        {
            bool exists = false;
            Regex regex = new Regex("^[A-Za-z0-9]\\S+$");
            service = new MemoryServer();
            if(!regex.IsMatch(nametag) || nametag.Length > 10 || nametag.Length < 4 )
            {
                exists = true;
                MessageBox.Show("El nametag solo puede tener entre 4 y 10 caracteres \n" +
                                " El nametag solo puede tener letras y numeros \n" +
                                "El nametag no puede llevar espacios");
            }else if (service.ExistsNametag(nametag))
            {
                exists = true;
                MessageBox.Show("El nametag ya esta registrado en el juego");
            }
            return exists;
        }

        private Boolean ExistsEqualEmail(string email)
        {
            bool exists = false;
            service = new MemoryServer();
            if (service.ExistsEmail(email))
            {
                exists = true;
                MessageBox.Show("El correo electronico ya esta registrado en el juego");
            }
            return exists;
        }     
    }
}
