using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        UserGame userGame = new UserGame();
        MemoryServer service;
        public Friends(UserGame _user)
        {
            userGame = _user;
            InitializeComponent();
            InitializeFriendsList();
        }

        private void InitializeFriendsList()
        {
            service = new MemoryServer();
            List<UserGame> friends = service.GetFriendsList(userGame.id);
            for(int i = 0; i < friends.Count(); i++)
            {
                string nametag = friends[i].nametag;
                listFriends.Items.Add(nametag);
            }
           
        }

        private void ClicExit(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
        }

        private void ClicAddFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listSearchUsers.SelectedItem;
            if (itemSelected != null)
            {
                string nametagAddressee = itemSelected.ToString();
                service = new MemoryServer();
                List<UserGame> addressee = service.GetUsersByInitialesOfNametag(nametagAddressee);
                if (!existsRequestImpediment(addressee[0]))
                {
                    bool added = service.AddFriendRequest(userGame.id, addressee[0].id);
                    if (added)
                    {
                        MessageBox.Show("Solicitud enviada con exito");
                    }
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar a alguien");
            }
            listSearchUsers.Items.Clear();
        }

        private bool existsRequestImpediment(UserGame addressee)
        {
            bool exists = false;
            service = new MemoryServer();
            MessageBox.Show(addressee.nametag);
            MessageBox.Show(userGame.nametag);
            if (addressee.nametag.Equals(userGame.nametag))
            {
                exists = true;
                MessageBox.Show("No puedes enviar una solicitud a ti mismo");
            }
            bool existsFriendship = service.ExistsFriendship(userGame.id, addressee.id);
            if (!existsFriendship)
            {
                bool exitsRequest = service.ExistsPendingRequest(userGame.id, addressee.id);
                if (exitsRequest)
                {
                    exists = true;
                    MessageBox.Show("Ya hay una solicitud pendiente");
                }
            }
            else
            {
                exists = true;
                MessageBox.Show("El jugador ya es tu amigo");
            }
            return exists;
        }

        private void ClicSearchFriend(object sender, RoutedEventArgs e)
        {
            if (!ExistsInvalidField())
            {
                MessageBox.Show("Buscando...");
                service = new MemoryServer();
                List<UserGame> coincidences = service.GetUsersByInitialesOfNametag(tbxNametag.Text);
                FillListSearchUsers(coincidences);
            }
        }

        private void FillListSearchUsers(List<UserGame> coincidences)
        {
            for (int i = 0; i < coincidences.Count(); i++)
            {
                string nametag = coincidences[i].nametag;
                listSearchUsers.Items.Add(nametag);
            }
        }

        private void ClicRemoveFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listFriends.SelectedItem;
            if(itemSelected != null)
            {
                service = new MemoryServer();
                string nametagFriend = itemSelected.ToString();
                List<UserGame> friend = service.GetUsersByInitialesOfNametag(nametagFriend);
                bool deleted = service.DeleteFriend(userGame.id, friend[0].id);
                if (deleted)
                {
                    listFriends.Items.Clear();
                    InitializeFriendsList();
                }
            }
            else
            {
                MessageBox.Show("Para eliminar debes seleccionar al amigo de la lista");
            }
        }

        private void ClicRequests(object sender, RoutedEventArgs e)
        {
            FriendRequests windowFriendRequests = new FriendRequests(userGame);
            windowFriendRequests.Show();
            this.Close();
        }

        private bool ExistsInvalidField()
        {
            bool exists = false;
            string userSearch = tbxNametag.Text;
            if (!string.IsNullOrEmpty(userSearch))
            {
                Regex regex = new Regex("^[A-Za-z0-9]\\S+$");
                if (!regex.IsMatch(userSearch) || userSearch.Length > 10)
                {
                    exists = true;
                    SendAlert(TypeError.INVALIDNAMETAG);
                }
            }
            else
            {
                exists = true;
                SendAlert(TypeError.EMPTYFIELD);
            }
            return exists;
        }

        private void SendAlert(TypeError typeError)
        {
            if (typeError == TypeError.EMPTYFIELD)
            {
                MessageBox.Show("El campo de texto está vacío");
            }

            if (typeError == TypeError.INVALIDNAMETAG)
            {
                MessageBox.Show("Existen caracteres inválidos en el nametag a buscar \n" +
                                "Recuerda que: \n" +
                                "El nametag solo puede tener más de 10 caracteres \n" +
                                "El nametag solo puede tener letras y numeros \n" +
                                "El nametag no puede llevar espacios");
            }
        }

        public enum TypeError
        {
            EMPTYFIELD, INVALIDNAMETAG
        }
    }
}
