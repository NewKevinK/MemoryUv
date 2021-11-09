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
    /// Lógica de interacción para FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : Window
    {
        UserGame usergame = new UserGame();
        MemoryServer service;
        public FriendRequests(UserGame _user)
        {
            InitializeComponent();
            usergame = _user;
            initializeListRequests();
        }

        private void initializeListRequests()
        {
            service = new MemoryServer();
            List<UserGame> usersRequesting = service.GetUsersRequesting(usergame.id);
            for(int i = 0; i < usersRequesting.Count(); i++)
            {
                string nametag = usersRequesting[i].nametag;
                listRequests.Items.Add(nametag);
            }
        }

        private void ClicAcceptFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listRequests.SelectedItem;
            if(itemSelected != null)
            {
                service = new MemoryServer();
                string nametagApplicant = itemSelected.ToString();
                List<UserGame> userApplicant = service.GetUsersByInitialesOfNametag(nametagApplicant);
                bool accepted = service.AcceptFriendRequest(userApplicant[0].id, usergame.id);
                if (accepted)
                {
                    bool added = service.AddFriend(userApplicant[0].id, usergame.id);
                    if (added)
                    {
                        MessageBox.Show("Tienes un nuevo amigo");
                        listRequests.Items.Clear();
                        initializeListRequests();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar una solicitud");
            }

        }

        private void ClicRejectFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listRequests.SelectedItem;
            if (itemSelected != null)
            {
                service = new MemoryServer();
                string nametagApplicant = itemSelected.ToString();
                List<UserGame> userApplicant = service.GetUsersByInitialesOfNametag(nametagApplicant);
                bool rejected = service.RejectFriendRequest(userApplicant[0].id, usergame.id);
                if (rejected)
                {
                    MessageBox.Show("Solicitud rechazada con exito");
                    listRequests.Items.Clear();
                    initializeListRequests();
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar una solicitud");
            }
        }

        private void ClicExit(object sender, RoutedEventArgs e)
        {
            Friends windowFriends = new Friends(usergame);
            windowFriends.Show();
            this.Close();
        }
    }
}
