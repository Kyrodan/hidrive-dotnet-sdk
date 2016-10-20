using System;
using Kyrodan.HiDrive.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Requests
{
    [TestClass]
    public abstract class BaseRequestTest
    {
        public const string TestFolderBase = "hidrive-dotnet-sdk-tests";

        public static IHiDriveAuthenticator Authenticator { get; }

        private static Random _random;

        public static Random Random
        {
            get
            {
                if (_random == null)
                    _random = new Random();

                return _random;
            }
        }

        static BaseRequestTest()
        {
            Authenticator = new HiDriveAuthenticator(ClientConfiguration.ClientId, ClientConfiguration.ClientSecret);
            Authenticator.AuthenticateByRefreshTokenAsync(ClientConfiguration.RefreshToken).Wait();
        }

        protected static HiDriveClient CreateClient()
        {
            return new HiDriveClient(Authenticator);
        }

        protected IHiDriveClient Client { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Client = CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Client = null;
        }

        protected static byte[] CreateRandomBytes(long length = 2048)
        {
            var buffer = new byte[length];
            Random.NextBytes(buffer);

            return buffer;
        }


    }
}
