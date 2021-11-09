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
using System.Data;
using Data;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        
        UserGame userGame = new UserGame();
        public Settings(UserGame _user)
        {
            InitializeComponent();
            userGame = _user;



        }

        

        private void CerrarSesionClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void RegresarClick(object sender, RoutedEventArgs e)
        {
            

            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
            

        }

        

        private void FiltroVista(object sender, RoutedEventArgs e)
        {
            if (chbVista.IsEnabled)
            {
                filtro.IsEnabled = true;
            }
            else
            {
                filtro.IsEnabled = false;
            }
            
        }

        
    }

    
}
