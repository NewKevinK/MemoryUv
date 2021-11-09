using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logic.FriendRequestLogic;

namespace Test
{
    [TestClass]
    public class FriendRequestTest
    {
        [TestMethod]
        public void TestAddFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.AddFriendRequest(1, 6);
            bool saved = false;
            if(status == Status.Sucess)
            {
                saved = true;
            }
            Assert.IsTrue(saved);
        }

        [TestMethod]
        public void TestAcceptFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.AcceptFriendRequest(1, 2);
            bool accepted = false;
            if (status == Status.Sucess)
            {
                accepted = true;
            }
            Assert.IsTrue(accepted);
        }

        [TestMethod]
        public void TestRejectFriendRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            Status status = friendRequestLogic.RejectFriendRequest(1, 5);
            bool rejected = false;
            if (status == Status.Sucess)
            {
                rejected = true;
            }
            Assert.IsTrue(rejected);
        }

        [TestMethod]
        public void TestGetUsersRequesting()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            List<UserGame> usersRequesting = friendRequestLogic.GetUsersRequesting(6);
            Assert.AreEqual(usersRequesting.Count(), 1);
        }

        [TestMethod]
        public void TestExistsPendingRequest()
        {
            FriendRequestLogic friendRequestLogic = new FriendRequestLogic();
            bool exists = friendRequestLogic.ExistsPendingRequest(1, 6);
            Assert.IsTrue(exists);
        }
    }
}
