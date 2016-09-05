using System.Threading.Tasks;
using Kyrodan.HiDrive.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests
{
    [TestClass]
    public class UserTests : BaseTest
    {
        [TestMethod]
        public async Task GetMe()
        {
            var result = await Client.User.Me.Get().ExecuteAsync();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Account);
            Assert.IsNotNull(result.Alias);
        }

        [TestMethod]
        public async Task GetMeWithAllFields()
        {
            var fields = new[] {
                User.Fields.Account,
                User.Fields.Alias,
                User.Fields.Description,
                User.Fields.EMail,
                User.Fields.Encrypted,
                User.Fields.Home,
                User.Fields.HomeId,
                User.Fields.IsAdmin,
                User.Fields.IsOwner,
                User.Fields.Language,
                User.Fields.Protocols,
            };

            var result = await Client.User.Me.Get(fields).ExecuteAsync();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Account);
            Assert.IsNotNull(result.Alias);
            Assert.IsNotNull(result.Description);
            Assert.IsNotNull(result.EMail);
            Assert.IsNotNull(result.Encrypted);
            Assert.IsNotNull(result.Home);
            Assert.IsNotNull(result.HomeId);
            Assert.IsNotNull(result.IsAdmin);
            Assert.IsNotNull(result.IsOwner);
            Assert.IsNotNull(result.Language);
            Assert.IsNotNull(result.Protocols);
        }
    }
}