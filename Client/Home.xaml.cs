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
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public string username;
        UserGame usergame = new UserGame();
        MemoryServer service;
        public Home(UserGame _user)
        {
            InitializeComponent();
            username = _user.nametag;
            lbUsername.Text = "" + username;
            usergame = _user;
        }

        private void ChatClick(object sender, RoutedEventArgs e)
        {
            Chat windowChat = new Chat(username);
            windowChat.Show();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();
            service.updateUserStatus(usergame.id, "Inactivo");
            this.Close();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(usergame);
            settings.Show();
            this.Close();



        }

        private void ArchievementClick(object sender, RoutedEventArgs e)
        {
            Logros logro = new Logros(usergame);
            logro.Show();
            this.Close();
        }

        private void FriendsClick(object sender, RoutedEventArgs e)
        {
            Friends windowFriends = new Friends(usergame);
            windowFriends.Show();
            this.Close();
        }
    }
}
