using System;
using System.IO;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Requests;
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
        public async Task UploadCreateNew()
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
        public async Task UploadOverwriteExisting()
        {
            // Arrange
            var initialContent = CreateRandomBytes();
            var initialStream = new MemoryStream(initialContent);

            var filename = "upload_test-" + Random.Next() + ".bin";
            var existingFile = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(initialStream);

            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);
            var sut = Client.File.Upload(filename, TestFolder, HomeId, UploadMode.CreateOrUpdate);

            // Act
            var result = await sut.ExecuteAsync(stream);

            // Assert
            Assert.AreEqual(filename, result.Name);
            Assert.AreEqual(content.LongLength, result.Size);
            Assert.AreEqual(existingFile.Id, result.Id);
        }

        [TestMethod]
        public async Task UploadFailOnExisting()
        {
            // Arrange
            var initialContent = CreateRandomBytes();
            var initialStream = new MemoryStream(initialContent);

            var filename = "upload_test-" + Random.Next() + ".bin";
            var existingFile = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(initialStream);

            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);
            var sut = Client.File.Upload(filename, TestFolder, HomeId);

            // Act
            try
            {
                var result = await sut.ExecuteAsync(stream);
            }

            // Assert
            catch (ServiceException ex)
            {
                Assert.IsNotNull(ex.Error);
                Assert.AreEqual("409", ex.Error.Code);
                return;
            }

            Assert.Fail("Expected Exception was not thrown.");
        }

        [TestMethod]
        [Ignore]
        public async Task Copy()
        {
        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var filename = "delete_test-" + Random.Next() + ".bin";

            var content = CreateRandomBytes();

            var stream = new MemoryStream(content);
            var file = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(stream);

            var sut = Client.File.Delete(TestFolder + "/" + filename, HomeId);

            // Act
            await sut.ExecuteAsync();

            // Assert
            // Just pass if no exception thrown
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