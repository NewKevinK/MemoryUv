using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Window, ChatService.IChatServiceCallback
    {
        public string userName;
        public ChatService.ChatServiceClient client;
        public bool isDataDirty = false;
        public Chat(string _username)
        {
            InitializeComponent();
            userName = _username;
            InstanceContext context = new InstanceContext(this);
            client = new ChatService.ChatServiceClient(context);
            client.Join(userName);
            lbId.Text = "Bienvenido "+ userName;
        }

        public void RecieveMessage(string user, string message)
        {
            string newMessage = $"{user} : {message}";
            messageDisplay.Items.Add(newMessage);
        }

        public void UsersUpdate(Dictionary<object, string>.ValueCollection users)
        {
            listUsersOnline.Items.Clear();
            cbUsers.Items.Clear();
            cbUsers.Items.Add("Todos");
            foreach (string user in users)
            {
                listUsersOnline.Items.Add(user);
                cbUsers.Items.Add(user);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChat.Text))
            {
                if (cbUsers.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione un destinatario");
                }
                else
                {
                    Object itemSelected = cbUsers.SelectedItem;
                    string receiver = itemSelected.ToString();
                    if (receiver == "Todos")
                    {
                        client.SendMessage(txtChat.Text);
                        txtChat.Text = "";
                    }else if(receiver == userName)
                    {
                        MessageBox.Show("No puedes enviarte un mensaje a ti mismo");
                    }
                    else
                    {
                        client.PrivateSendMessage(txtChat.Text, receiver);
                        txtChat.Text = "";
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isDataDirty = true;
            if (this.isDataDirty)
            {
                client.leave(userName);
            }
        }
    }
}
