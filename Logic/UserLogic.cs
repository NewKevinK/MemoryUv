using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class UserLogic
    {
        public Status AddUser(string email, string password, string nametag)
        {
            Authentication authentication = new Authentication();
            string passwordHashed = authentication.ComputeSHA256Hash(password);
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    UserGame userGame = new UserGame()
                    {
                        email = email,
                        password = passwordHashed,
                        nametag = nametag
                    };
                    context.UsersGame.Add(userGame);
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
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

        public Status UpdatePasswordUser(int idUser, string newPassword)
        {
            Status status = Status.Failed;
            Authentication authentication = new Authentication();
            string newPasswordHashed = authentication.ComputeSHA256Hash(newPassword);
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if(coincidences.Count() > 0)
                    {
                        UserGame userGame = coincidences.First();
                        userGame.password = newPasswordHashed;
                    }
                    int numberChanges = context.SaveChanges();
                    if(numberChanges > 0)
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

        public Status updateUserStatus(int idUser, string userStatus) 
        {
            Status status = Status.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if (coincidences.Count() > 0)
                    {
                        UserGame userGame = coincidences.First();
                        userGame.status = userStatus;
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

        public UserGame GetUserGameById(int idUser)
        {
            UserGame userGame = new UserGame();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.id == idUser select UserGame;
                    if(coincidences.Count() > 0)
                    {
                        userGame = coincidences.First();
                    }
                    else
                    {
                        userGame = null;
                    }
                }
            }
            catch (DbException)
            {

            }
            return userGame;
        }

        public UserGame GetUserGameByEmail(string email)
        {
            UserGame userGame = null;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = from UserGame in context.UsersGame where UserGame.email == email select UserGame;
                    if (coincidences.Count() > 0)
                    {
                        userGame = coincidences.First();
                    }
                }
            }
            catch (DbException)
            {

            }
            return userGame;
        }

        public List<UserGame> GetUsersByInitialesOfNametag(string nametag)
        {
            List<UserGame> users = new List<UserGame>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame where UserGame.nametag.StartsWith(nametag) select UserGame).ToList();
                    users = coincidences;
                } 
            }
            catch (DbException)
            {

            }
            return users;
        }

        public VerificationStatus VerifyMailExistence(string email)
        {
            VerificationStatus status = VerificationStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame
                                        where UserGame.email == email
                                        select UserGame).Count();
                    if (coincidences > 0)
                    {
                        status = VerificationStatus.Sucess;
                    }
                }
            }
            catch (DbException )
            {

            }
            
            return status;
        }

        public VerificationStatus VerifyNametagExistence(string nametag)
        {
            VerificationStatus status = VerificationStatus.Failed;
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from UserGame in context.UsersGame
                                        where UserGame.nametag == nametag
                                        select UserGame).Count();
                    if (coincidences > 0)
                    {
                        status = VerificationStatus.Sucess;
                    }
                }
            } catch (DbException)
            {

            }
            
            return status;
        }
        

        public enum Status
        {
            Sucess = 0, 
            Failed
        }

        public enum VerificationStatus
        {
            Sucess = 0,
            Failed
        }


    }
}
