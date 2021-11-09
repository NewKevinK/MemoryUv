using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FriendRequestLogic
    {

        public Status AddFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    FriendRequest friendRequest = new FriendRequest()
                    {
                        idApplicant = idApplicant,
                        idReceiver = idReceiver,
                        requestStatus = "Pendiente"
                    };
                    context.FriendRequests.Add(friendRequest);
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public Status AcceptFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver select FriendRequest;
                    if(coincidences.Count() > 0)
                    {
                        FriendRequest friendRequest = coincidences.First();
                        friendRequest.requestStatus = "Aceptada";
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public Status RejectFriendRequest(int idApplicant, int idReceiver)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver select FriendRequest;
                    if (coincidences.Count() > 0)
                    {
                        FriendRequest friendRequest = coincidences.First();
                        friendRequest.requestStatus = "Rechazada";
                    }
                    int numberChanges = context.SaveChanges();
                    if (numberChanges > 0)
                    {
                        status = Status.Sucess;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public List<UserGame> GetUsersRequesting(int idUser)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from FriendRequest in context.FriendRequests where FriendRequest.idReceiver == idUser && FriendRequest.requestStatus == "Pendiente" select FriendRequest).ToList();
                    UserLogic userLogic = new UserLogic();
                    for (int i = 0; i < coincidences.Count(); i++)
                    {
                        UserGame userGame = new UserGame();
                        userGame = userLogic.GetUserGameById(coincidences[i].idApplicant);
                        users.Add(userGame);
                    }
                }
            }
            catch (DbException)
            {

            }
            return users;
        }

        public bool ExistsPendingRequest(int idApplicant, int idReceiver)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from FriendRequest in context.FriendRequests where ((FriendRequest.idApplicant == idApplicant && FriendRequest.idReceiver == idReceiver) || 
                                       (FriendRequest.idReceiver == idApplicant && FriendRequest.idApplicant == idReceiver)) && FriendRequest.requestStatus == "Pendiente" select FriendRequest;
                    if(coincidences.Count() > 0)
                    {
                        exists = true;
                    }

                }
            }
            catch (DbException)
            {

            }
            return exists;
        }

        public enum Status
        {
            Sucess = 0,
            Failed
        }
    }
}
