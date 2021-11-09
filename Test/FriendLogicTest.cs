using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows;
using static Logic.FriendLogic;

namespace Test
{
    [TestClass]
    public class FriendLogicTest
    {

        [TestMethod]
        public void TestAddFriend()
        {
            FriendLogic friendLogic = new FriendLogic();
            Status status = friendLogic.AddFriend(6, 1);
            bool saved = false;
            if (status == Status.Success)
            {
                saved = true;
            }
            Assert.IsTrue(saved);
        }

        [TestMethod]
        public void TestDeleteFriend()
        {
            FriendLogic friendLogic = new FriendLogic();
            Status status = friendLogic.DeleteFriend(1, 5);
            bool deleted = false;
            if (status == Status.Success)
            {
                deleted = true;
            }
            Assert.IsTrue(deleted);
        }

        [TestMethod]
        public void TestGetFriendsList()
        {
            FriendLogic friendLogic = new FriendLogic();
            List<UserGame> users = friendLogic.GetFriendsList(1);
            Assert.AreEqual(0, users.Count);
        }

        [TestMethod]
        public void TestExistsFriendship()
        {
            FriendLogic friendLogic = new FriendLogic();
            bool exists = friendLogic.ExistsFriendship(6, 1);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void SecondTestExistsFriendship()
        {
            FriendLogic friendLogic = new FriendLogic();
            bool exists = friendLogic.ExistsFriendship(1, 6);
            Assert.IsTrue(exists);
        }

    }
}
