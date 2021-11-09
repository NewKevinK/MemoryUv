using Data;
using Logic;
using System;
using Host;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using static Logic.UserLogic;
using static Logic.FriendLogic;

namespace Host
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(string user, string message);

        [OperationContract(IsOneWay = true)]
        void UsersUpdate(Dictionary<IChatClient, string>.ValueCollection users);
    }

    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Join(string username);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message);

        [OperationContract(IsOneWay = true)]
        void Leave(string username);

        [OperationContract(IsOneWay = true)]
        void PrivateSendMessage(string message, string username);

    }

    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserGame GetLoggerUser(string email, string password);

        [OperationContract]
        Boolean RegisterUser(string email, string password, string nametag);

        [OperationContract]
        Boolean ExistsEmail(string email);

        [OperationContract]
        Boolean ExistsNametag(string nametag);

        [OperationContract]
        String SendEmail(string email);

        [OperationContract]
        bool UpdatePassword(int idUser, string newPassword);

        [OperationContract]
        UserGame GetUserById(int idUser);

        [OperationContract]
        UserGame GetUserByEmail(string email);

        [OperationContract]
        List<UserGame> GetUsersByInitialesOfNametag(string nametag);

        [OperationContract]
        bool updateUserStatus(int idUser, string userStatus);
    }

    [ServiceContract]
    public interface IFriendService
    {
        [OperationContract]
        bool AddFriend(int idUser, int idFriend);

        [OperationContract]
        bool DeleteFriend(int idUser, int idFriend);

        [OperationContract]
        List<UserGame> GetFriendsList(int idUser);

        [OperationContract]
        bool ExistsFriendship(int idUser, int idFriend);
    }

    [ServiceContract]
    public interface IFriendRequestService
    {
        [OperationContract]
        bool AddFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        bool AcceptFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        bool RejectFriendRequest(int idApplicant, int idReceiver);

        [OperationContract]
        List<UserGame> GetUsersRequesting(int idUser);

        [OperationContract]
        bool ExistsPendingRequest(int idApplicant, int idReceiver);
    }

    [ServiceContract]
    public interface IStatisticService
    {
        [OperationContract]
        List<StatisticUser> GetBetterUser();

        [OperationContract]
        bool GetStatisticUser(int idUser, int numAchievement);

        [OperationContract]
        bool AddOneWinGame(int idUser);

        [OperationContract]
        bool AddOneLoseGame(int idUser);


    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class MemoryServer : IChatService, IUserService, IFriendService, IFriendRequestService
    {
        Dictionary<IChatClient, string> _users = new Dictionary<IChatClient, string>();
        public void Join(string username)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            _users[connection] = username;
            Console.WriteLine("El usuario " + username + " se conecto...");
            foreach (var userConnection in _users.Keys)
            {
                userConnection.UsersUpdate(_users.Values);
            }
        }

        public void SendMessage(string message)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            string user;
            if(!_users.TryGetValue(connection, out user))
            {
                return;
            }
            foreach (var other in _users.Keys)
            {
                if(other == connection)
                {
                    other.RecieveMessage(user, message);
                }
                other.RecieveMessage(user, message);
                //implementar junto a private...
            }
        }
        public void Leave(string username)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            Console.WriteLine("El usuario " + _users[connection] + " se desconecto!");
            _users.Remove(connection);
            foreach (var userConnection in _users.Keys)
            {
                userConnection.UsersUpdate(_users.Values);
            }
        }

        public void PrivateSendMessage(string message, string username)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            string user;
            string userPrivate;
            if (!_users.TryGetValue(connection, out user))
            {
                return;
            }

            foreach (var uPrivate in _users.Keys)
            {
                if (_users.TryGetValue(uPrivate, out userPrivate))
                {
                    if (userPrivate == username)
                    {
                        uPrivate.RecieveMessage(user, message);
                    }
                    
                }
            }

        }

        public UserGame GetLoggerUser(string email, string password)
        {
            Authentication authentication = new Authentication();
            UserGame user = authentication.Login(email, password);
            return user;
        }

        public Boolean RegisterUser(string email, string password, string nametag)
        {
            bool saved = false;
            UserLogic userLogic = new UserLogic();
            UserLogic.Status status = userLogic.AddUser(email, password, nametag);
            if (status == UserLogic.Status.Sucess)
            {
                saved = true;
            }
            return saved;
        }

        public bool ExistsEmail(string email)
        {
            bool exists = false;
            UserLogic userLogic = new UserLogic();
            VerificationStatus status = userLogic.VerifyMailExistence(email);
            if (status == VerificationStatus.Sucess)
            {
                exists = true;
            }
            return exists;
        }

        public bool ExistsNametag(string nametag)
        {
            bool exists = false;
            UserLogic userLogic = new UserLogic();
            VerificationStatus status = userLogic.VerifyNametagExistence(nametag);
            if (status == VerificationStatus.Sucess)
            {
                exists = true;
            }
            return exists;
        }

        public String SendEmail(string email)
        {
            Authentication authentication = new Authentication();
            String code = authentication.RandomString();
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            mmsg.To.Add(email);
            mmsg.Subject = "CODE UV";
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
            mmsg.Body = code;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true;

            mmsg.From = new System.Net.Mail.MailAddress("memoryuv@gmail.com");
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("memoryuv@gmail.com", "0qwerty0");

            client.Port = 587;
            client.EnableSsl = true;

            client.Host = "smtp.gmail.com";

            try
            {
                client.Send(mmsg);
                
            }
            catch(Exception)
            {
                MessageBox.Show("Error al enviar");
            }

            return code;
        }

        public bool UpdatePassword(int idUser, string newPassword)
        {
            bool updated = false;
            UserLogic userLogic = new UserLogic();
            UserLogic.Status status = userLogic.UpdatePasswordUser(idUser, newPassword);
            if(status == UserLogic.Status.Sucess)
            {
                updated = true;
            }
            return updated;
        }

        public bool updateUserStatus(int idUser, string userStatus)
        {
            bool updated = false;
            UserLogic userLogic = new UserLogic();
            UserLogic.Status status = userLogic.updateUserStatus(idUser, userStatus);
            if (status == UserLogic.Status.Sucess)
            {
                updated = true;
            }
            return updated;
        }

        public UserGame GetUserById(int idUser)
        {
            UserLogic userLogic = new UserLogic();
            UserGame userGame = userLogic.GetUserGameById(idUser);
            return userGame;
        }

        public UserGame GetUserByEmail(string email)
        {
            UserLogic userLogic = new UserLogic();
            UserGame userGame = userLogic.GetUserGameByEmail(email);
            return userGame;
        }

        public List<UserGame> GetUsersByInitialesOfNametag(string nametag)
        {
            UserLogic userLogic = new UserLogic();
            List<UserGame> users = userLogic.GetUsersByInitialesOfNametag(nametag);
            return users;
        }

        public bool AddFriend(int idUser, int idFriend)
        {
            FriendLogic friendLogic = new FriendLogic();
            bool saved = false;
            FriendLogic.Status status = friendLogic.AddFriend(idUser, idFriend);
            if(status == FriendLogic.Status.Success)
            {
                saved = true;
            }
            return saved;
        }

        public bool DeleteFriend(int idUser, int idFriend)
        {
            FriendLogic friendLogic = new FriendLogic();
            bool deleted = false;
            FriendLogic.Status status = friendLogic.DeleteFriend(idUser, idFriend);
            if (status == FriendLogic.Status.Success)
            {
                deleted = true;
            }
            return deleted;
        }

        public List<UserGame> GetFriendsList(int idUser)
        {
            FriendLogic friendLogic = new FriendLogic();
            List<UserGame> users = friendLogic.GetFriendsList(idUser);
            return users;
        }

        public bool ExistsFriendship(int idUser, int idFriend)
        {
            FriendLogic friendLogic = new FriendLogic();
            bool exists = friendLogic.ExistsFriendship(idUser, idFriend);
            return exists;
        }

        public bool AddFriendRequest(int idApplicant, int idReceiver)
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool saved = false;
            FriendRequestLogic.Status status = friendRequestLogic.AddFriendRequest(idApplicant, idReceiver);
            if(status == FriendRequestLogic.Status.Sucess)
            {
                saved = true;
            }
            return saved;
        }

        public bool AcceptFriendRequest(int idApplicant, int idReceiver)
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool accepted = false;
            FriendRequestLogic.Status status = friendRequestLogic.AcceptFriendRequest(idApplicant, idReceiver);
            if (status == FriendRequestLogic.Status.Sucess)
            {
                accepted = true;
            }
            return accepted;
        }

        public bool RejectFriendRequest(int idApplicant, int idReceiver)
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool rejected = false;
            FriendRequestLogic.Status status = friendRequestLogic.RejectFriendRequest(idApplicant, idReceiver);
            if (status == FriendRequestLogic.Status.Sucess)
            {
                rejected = true;
            }
            return rejected;
        }

        public List<UserGame> GetUsersRequesting(int idUser)
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            List<UserGame> usersRequesting = friendRequestLogic.GetUsersRequesting(idUser);
            return usersRequesting;
        }

        public bool ExistsPendingRequest(int idApplicant, int idReceiver)
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool exists = friendRequestLogic.ExistsPendingRequest(idApplicant, idReceiver);
            return exists;
        }

        public List<StatisticUser> GetBetterUser()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            List<StatisticUser> users = statisticUserLogic.GetBetterUsers();
            return users;
        }

        public bool GetStatisticUser(int idUser, int numAchievement)
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            bool value = statisticUserLogic.GetStatisticUser(idUser, numAchievement);

            return true;
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Host.MemoryServer)))
            {
                host.Open();
                Console.WriteLine("Servidor listo...");
                Console.WriteLine("Presiona cualquier letra para finalizar...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
