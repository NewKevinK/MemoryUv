using Data;
using Host;
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
    /// Lógica de interacción para Logros.xaml
    /// </summary>
    public partial class Logros : Window
    {
        UserGame userGame = new UserGame();
        MemoryServer service;

        public Logros(UserGame _user)
        {
            InitializeComponent();
            InitializeArchievement();
            userGame = _user;
        }

        private void InitializeArchievement()
        {
            service = new MemoryServer();
            if (service.GetStatisticUser(userGame.id, 1))
            {
                lbAchievement1.IsEnabled = true;
                imgAchievement1.Opacity = 100;
            }
            if (service.GetStatisticUser(userGame.id, 2))
            {
                lbAchievement2.IsEnabled = true;
                imgAchievement2.Opacity = 100;
            }
            if (service.GetStatisticUser(userGame.id, 3))
            {
                lbAchievement3.IsEnabled = true;
                imgAchievement3.Opacity = 100;
            }
            if (service.GetStatisticUser(userGame.id, 4))
            {
                lbAchievement4.IsEnabled = true;
                imgAchievement4.Opacity = 100;
            }
            if (service.GetStatisticUser(userGame.id, 5))
            {
                lbAchievement5.IsEnabled = true;
                imgAchievement5.Opacity = 100;
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }



        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
        }
    }
}
