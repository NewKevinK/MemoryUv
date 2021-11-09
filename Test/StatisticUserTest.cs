using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestClass]
    public class StatisticUserTest
    {

        [TestMethod]
        public void TestGetBetterUsers()
        {
            StatisticUserLogic statisticUserLogic = new StatisticUserLogic();
            List<StatisticUser> statisticUsers = statisticUserLogic.GetBetterUsers();
            bool isOrdered = false;
            if((statisticUsers[0].totalWins > statisticUsers[1].totalWins) && (statisticUsers[1].totalWins > statisticUsers[2].totalWins) && (statisticUsers[2].totalWins > statisticUsers[3].totalWins))
            {
                isOrdered = true;
            }
            Assert.IsTrue(isOrdered);
        }

    }
}
