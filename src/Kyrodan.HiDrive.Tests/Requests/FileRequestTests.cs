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
        public static void ClassCleanup()
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

            // Act
            var sut = Client.File.Download(TestFolder + "/" + filename, HomeId);
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

            // Act
            var sut = Client.File.Upload(filename, TestFolder, HomeId);
            var result = await sut.ExecuteAsync(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(filename, result.Name);
            Assert.AreEqual(content.LongLength, result.Size);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.IsNotNull(result.MimeType);
            Assert.IsNotNull(result.ModifiedDateTime);
            Assert.IsNotNull(result.ParentId);
            Assert.IsNotNull(result.IsReadable);
            Assert.IsNotNull(result.Type);
            Assert.IsNotNull(result.IsWritable);

        }

        [TestMethod]
        public async Task UploadOverwriteExisting()
        {
            // Arrange
            var initialContent = CreateRandomBytes();
            var initialStream = new MemoryStream(initialContent);

            var filename = "upload_test-" + Random.Next() + ".bin";
            var existingFile = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(initialStream);

            var content = CreateRandomBytes(4096);
            var stream = new MemoryStream(content);

            // Act
            var sut = Client.File.Upload(filename, TestFolder, HomeId, UploadMode.CreateOrUpdate);
            var result = await sut.ExecuteAsync(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingFile.Name, result.Name);
            Assert.AreEqual(content.LongLength, result.Size);
            Assert.AreEqual(existingFile.Id, result.Id);
            Assert.AreEqual(existingFile.Path, result.Path);
            Assert.AreEqual(existingFile.CreatedDateTime, result.CreatedDateTime);
            Assert.IsNotNull(result.MimeType);
            Assert.IsNotNull(result.ModifiedDateTime);
            Assert.AreEqual(existingFile.ParentId, result.ParentId);
            Assert.AreEqual(existingFile.IsReadable, result.IsReadable);
            Assert.AreEqual(existingFile.Type, result.Type);
            Assert.AreEqual(existingFile.IsWritable, result.IsWritable);
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

            // Act
            var sut = Client.File.Upload(filename, TestFolder, HomeId);
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
        public async Task Copy()
        {
            // Arrange
            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);

            var sourceFilename = "copy_test-source-" + Random.Next() + ".bin";
            var sourcePath = TestFolder + "/" + sourceFilename;
            var source = await Client.File.Upload(sourceFilename, TestFolder, HomeId).ExecuteAsync(stream);

            var destFilename = "copy_test-dest-" + Random.Next() + ".bin";
            var destPath = TestFolder + "/" + destFilename;

            // Act
            var sut = Client.File.Copy(sourcePath, HomeId, destPath, HomeId);
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.IsNotNull(result.MimeType);
            Assert.IsNotNull(result.ModifiedDateTime);
            Assert.AreEqual(destFilename, result.Name);
            Assert.IsNotNull(result.ParentId);
            Assert.IsNotNull(result.IsReadable);
            Assert.AreEqual(content.LongLength, result.Size);
            Assert.IsNotNull(result.Type);
            Assert.IsNotNull(result.IsWritable);
        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var filename = "delete_test-" + Random.Next() + ".bin";

            var content = CreateRandomBytes();

            var stream = new MemoryStream(content);
            var file = await Client.File.Upload(filename, TestFolder, HomeId).ExecuteAsync(stream);


            // Act
            var sut = Client.File.Delete(TestFolder + "/" + filename, HomeId);
            await sut.ExecuteAsync();

            // Assert
            // Just pass if no exception thrown
        }

        [TestMethod]
        public async Task Rename()
        {
            // Arrange
            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);

            var sourceFilename = "rename_test-source-" + Random.Next() + ".bin";
            var sourcePath = TestFolder + "/" + sourceFilename;
            var source = await Client.File.Upload(sourceFilename, TestFolder, HomeId).ExecuteAsync(stream);

            var destFilename = "rename_test-dest-" + Random.Next() + ".bin";

            // Act
            var sut = Client.File.Rename(sourcePath, HomeId, destFilename);
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(destFilename, result.Name);
            Assert.AreEqual(source.Size, result.Size);
            Assert.AreEqual(source.Id, result.Id);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.AreEqual(source.MimeType, result.MimeType);
            Assert.AreEqual(source.ModifiedDateTime, result.ModifiedDateTime);
            Assert.AreEqual(source.ParentId, result.ParentId);
            Assert.AreEqual(source.IsReadable, result.IsReadable);
            Assert.AreEqual(source.Type, result.Type);
            Assert.AreEqual(source.IsWritable, result.IsWritable);
        }

        [TestMethod]
        public async Task Move()
        {
            // Arrange
            var content = CreateRandomBytes();
            var stream = new MemoryStream(content);

            var sourceFilename = "move_test-source-" + Random.Next() + ".bin";
            var sourcePath = TestFolder + "/" + sourceFilename;
            var source = await Client.File.Upload(sourceFilename, TestFolder, HomeId).ExecuteAsync(stream);

            var destFilename = "move_test-dest-" + Random.Next() + ".bin";
            var destPath = TestFolder + "/" + destFilename;

            // Act
            var sut = Client.File.Move(sourcePath, HomeId, destPath, HomeId);
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(destFilename, result.Name);
            Assert.AreEqual(source.Size, result.Size);
            Assert.AreEqual(source.Id, result.Id);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.AreEqual(source.MimeType, result.MimeType);
            Assert.AreEqual(source.ModifiedDateTime, result.ModifiedDateTime);
            Assert.AreEqual(source.ParentId, result.ParentId);
            Assert.AreEqual(source.IsReadable, result.IsReadable);
            Assert.AreEqual(source.Type, result.Type);
            Assert.AreEqual(source.IsWritable, result.IsWritable);
        }

    }
}