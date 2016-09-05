using Kyrodan.HiDrive.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests
{
    [TestClass]
    public class AssemblySetup
    {
        public static IHiDriveAuthenticator Authenticator { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Authenticator = new HiDriveAuthenticator(ClientConfiguration.ClientId, ClientConfiguration.ClientSecret);
            Authenticator.AuthenticateByRefreshTokenAsync(ClientConfiguration.RefreshToken).Wait();
        }
    }
}