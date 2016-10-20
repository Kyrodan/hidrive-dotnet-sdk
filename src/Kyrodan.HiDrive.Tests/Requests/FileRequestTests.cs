using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Requests
{
    [TestClass]
    public class FileRequestTests : BaseRequestTest
    {
        private static string TestFolder { get; set; }

        private static string HomeId { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var client = CreateClient();
            HomeId = client.GetHomeId().Result;

            TestFolder = TestFolderBase + "-" + Random.Next();
            client.Directory.Create(TestFolder, HomeId).ExecuteAsync().Wait();
        }

        [ClassCleanup]
        public static async void ClassCleanup()
        {
            var client = CreateClient();
            client.Directory.Delete(TestFolder, HomeId, true).ExecuteAsync().Wait();

            HomeId = null;
            TestFolder = null;
        }

        [TestMethod]
        public async Task Download()
        {
            // Arrange
            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);

            var filename = "download_test-" + Random.Next() + ".bin";
            var uploadedFile = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(stream);

            var sut = Client.File.Download(TestFolder + "/" + filename, HomeId);

            // Act
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.AreEqual(content.LongLength, result.Length);
            for (var i = 0; i < content.LongLength; i++)
            {
                Assert.AreEqual(content[i], result.ReadByte());
            }

        }

        [TestMethod]
        [Ignore]
        public async Task Upload()
        {   
            // Arrange
            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);

            var filename = "upload_test-" + Random.Next() + ".bin";
            var sut = Client.File.Upload(filename, TestFolder, HomeId);

            // Act
            var result = await sut.ExecuteAsync(stream);

            // Assert
            Assert.AreEqual(filename, result.Name);
            Assert.AreEqual(content.LongLength, result.Size);
        }

        [TestMethod]
        [Ignore]
        public async Task Copy()
        {
        }

        [TestMethod]
        [Ignore]
        public async Task Delete()
        {
        }

        [TestMethod]
        [Ignore]
        public async Task Rename()
        {
        }

        [TestMethod]
        [Ignore]
        public async Task Move()
        {
        }

    }
}