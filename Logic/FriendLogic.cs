using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FriendLogic
    {
        public Status AddFriend(int idUser, int idFriend)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    Friend friend = new Friend()
                    {
                        idUser = idUser,
                        idFriend = idFriend
                    };
                    context.Friends.Add(friend);
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
                    {
                        status = Status.Success;
                    }
                }
            }
            catch (DbException)
            {

            }
            return status;
        }

        public Status DeleteFriend(int idUser, int idFriend)
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from Friend in context.Friends where Friend.idUser == idUser && Friend.idFriend == idFriend select Friend;
                    if(coincidences.Count() > 0)
                    {
                        Friend friend = coincidences.First();
                        context.Friends.Remove(friend);
                        int numberChanges = context.SaveChanges();
                        if(numberChanges > 0)
                        {
                            status = Status.Success;
                        }
                    }
                }
            }
            catch (DbException)
            {

            }

            return status;
        }

        public List<UserGame> GetFriendsList(int idUser)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from Friend in context.Friends where Friend.idUser == idUser || Friend.idFriend == idUser select Friend).ToList();
                    UserLogic userLogic = new UserLogic();
                    for(int i = 0; i < coincidences.Count(); i++)
                    {
                        UserGame userGame = new UserGame();
                        if(coincidences[i].idUser == idUser)
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idFriend);
                        }
                        else
                        {
                            userGame = userLogic.GetUserGameById(coincidences[i].idUser);
                        }
                        users.Add(userGame);
                    }
                }
            }
            catch (DbException)
            {

            }
            return users;
        }

        public bool ExistsFriendship(int idUser, int idFriend)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    var concidences = (from Friend in context.Friends where (Friend.idUser == idUser || Friend.idFriend == idUser) && (Friend.idFriend == idFriend || Friend.idUser == idFriend)
                                       select Friend).Count();
                    if(concidences > 0)
                    {
                        exists = true;
                    }
                }
            } catch (DbException)
            {

            }
            return exists;
        }

        public enum Status
        {
            Success = 0,
            Failed
        }
    }
}
