using Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class StatisticUserLogic
    {
        public List<StatisticUser> GetBetterUsers()
        {
            List<StatisticUser> users = new List<StatisticUser>();
            try
            {
                using (var context = new MemoryModel())
                {
                    var coincidences = (from StatisticUser in context.StatisticsUser orderby StatisticUser.totalWins descending select StatisticUser).ToList();
                    users = coincidences;
                }
            }
            catch (DbException)
            {

            }
            return users;
        }

        public bool GetStatisticUser(int idUser, int numAchievement)
        {
            bool exists = false;
            try
            {
                using (var context = new MemoryModel())
                {
                    if (numAchievement == 1)
                    {
                        //var concidences = ();

                        /* if (concidences > 0)
                         {
                             exists = true;
                         } */
                    }
                    else if (numAchievement == 2)
                    {

                    }
                    else if (numAchievement == 3)
                    {

                    }
                    else if (numAchievement == 4)
                    {

                    }
                    else if (numAchievement == 5)
                    {

                    }



                }
            }
            catch (DbException)
            {

            }
            return exists;
        }




    }
}
