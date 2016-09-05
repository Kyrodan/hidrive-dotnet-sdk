using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        protected IHiDriveClient Client { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Client = new HiDriveClient(AssemblySetup.Authenticator);
        }


        [TestCleanup]
        public void Cleanup()
        {
            Client = null;
        }
    }
}
