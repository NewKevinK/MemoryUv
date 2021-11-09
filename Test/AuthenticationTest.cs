using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class AuthenticationTest
    {
        [TestMethod]
        public void TestLogin()
        {
            Authentication authentication = new Authentication();
            UserGame user = authentication.Login("david.mijangos.08@gmail.com", "Mijangos12");
            Assert.AreEqual(user.nametag, "Mijangos08");
        }

        [TestMethod]
        public void TestComputeSHA256Hash()
        {
            Authentication authentication = new Authentication();
            string stringHashed = authentication.ComputeSHA256Hash("Gato");
            Assert.AreEqual(stringHashed, "a9baf529fb363b918abc2832c7df4c41c55c0906f13d4b054eb205529806efa3");
        }
    }
}
