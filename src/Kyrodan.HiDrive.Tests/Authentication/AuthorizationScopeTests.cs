using Kyrodan.HiDrive.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Authentication
{
    [TestClass]
    public class AuthorizationScopeTests
    {
        [TestMethod]
        public void User_ReadWrite()
        {
            var sut = new AuthorizationScope(AuthorizationRole.User, AuthorizationPermission.ReadWrite);
            var result = sut.ToString();
            Assert.AreEqual("user,rw", result);
        }

        [TestMethod]
        public void User_ReadOnly()
        {
            var sut = new AuthorizationScope(AuthorizationRole.User, AuthorizationPermission.ReadOnly);
            var result = sut.ToString();
            Assert.AreEqual("user,ro", result);
        }

        [TestMethod]
        public void Admin_ReadWrite()
        {
            var sut = new AuthorizationScope(AuthorizationRole.Admin, AuthorizationPermission.ReadWrite);
            var result = sut.ToString();
            Assert.AreEqual("admin,rw", result);
        }

        [TestMethod]
        public void Admin_ReadOnly()
        {
            var sut = new AuthorizationScope(AuthorizationRole.Admin, AuthorizationPermission.ReadOnly);
            var result = sut.ToString();
            Assert.AreEqual("admin,ro", result);
        }

        [TestMethod]
        public void Owner_ReadWrite()
        {
            var sut = new AuthorizationScope(AuthorizationRole.Owner, AuthorizationPermission.ReadWrite);
            var result = sut.ToString();
            Assert.AreEqual("owner,rw", result);
        }

        [TestMethod]
        public void Owner_ReadOnly()
        {
            var sut = new AuthorizationScope(AuthorizationRole.Owner, AuthorizationPermission.ReadOnly);
            var result = sut.ToString();
            Assert.AreEqual("owner,ro", result);
        }

    }
}