using Kyrodan.HiDrive.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Requests
{
    [TestClass]
    public abstract class BaseRequestTest
    {
        public static IHiDriveAuthenticator Authenticator { get; }

        static BaseRequestTest()
        {
            Authenticator = new HiDriveAuthenticator(ClientConfiguration.ClientId, ClientConfiguration.ClientSecret);
            Authenticator.AuthenticateByRefreshTokenAsync(ClientConfiguration.RefreshToken).Wait();
        }

        protected IHiDriveClient Client { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Client = new HiDriveClient(Authenticator);
        }


        [TestCleanup]
        public void Cleanup()
        {
            Client = null;
        }
    }
}
