using System;
using System.IO;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Models;
using Kyrodan.HiDrive.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Requests
{
    [TestClass]
    public class DirectoryRequestTests : BaseRequestTest
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
        public async Task Get()
        {
            // Arrange
            var sut = Client.Directory.Get(null, HomeId);

            // Act
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Members);
            Assert.IsNotNull(result.Path);
        }

        [TestMethod]
        public async Task GetWithAllFields()
        {
            // Arrange
            var fields = new[]
            {
                DirectoryBaseItem.Fields.Path,
                DirectoryBaseItem.Fields.CHash,
                DirectoryBaseItem.Fields.CreatedDateTime,
                DirectoryBaseItem.Fields.HasDirectories,
                DirectoryBaseItem.Fields.Id,
                DirectoryBaseItem.Fields.IsReadable,
                DirectoryBaseItem.Fields.IsWritable,
                DirectoryBaseItem.Fields.Members,
                DirectoryBaseItem.Fields.MetaHash,
                DirectoryBaseItem.Fields.MetaOnlyHash,
                DirectoryBaseItem.Fields.ModifiedDateTime,
                DirectoryBaseItem.Fields.Name,
                DirectoryBaseItem.Fields.NameHash,
                DirectoryBaseItem.Fields.NumberOfMembers,
                DirectoryBaseItem.Fields.ParentId,
                DirectoryBaseItem.Fields.Size,
                DirectoryBaseItem.Fields.Type
            };

            var sut = Client.Directory.Get(null, HomeId, null, fields);

            // Act
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CHash);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.IsNotNull(result.HasDirectories);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.IsReadable);
            Assert.IsNotNull(result.IsWritable);
            Assert.IsNotNull(result.Members);
            Assert.IsNotNull(result.MetaHash);
            Assert.IsNotNull(result.MetaOnlyHash);
            Assert.IsNotNull(result.ModifiedDateTime);
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.NameHash);
            Assert.IsNotNull(result.NumberOfMembers);
            Assert.IsNotNull(result.ParentId);
            Assert.IsNotNull(result.Size);
            Assert.IsNotNull(result.Type);

        }

        [TestMethod]
        [Ignore]
        public async Task GetHome()
        {
        }

        [TestMethod]
        public async Task Create()
        {
            // Arrange
            var filename = "create_test-" + Random.Next();
            var path = TestFolder + "/" + filename;

            var sut = Client.Directory.Create(path, HomeId);

            // Act
            var result = await sut.ExecuteAsync();
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CHash);
            Assert.IsNotNull(result.CreatedDateTime);
            Assert.IsNotNull(result.HasDirectories);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.IsReadable);
            Assert.IsNotNull(result.IsWritable);
            Assert.IsNotNull(result.MetaHash);
            Assert.IsNotNull(result.MetaOnlyHash);
            Assert.IsNotNull(result.ModifiedDateTime);
            Assert.AreEqual(filename, result.Name);
            Assert.IsNotNull(result.NameHash);
            Assert.IsNotNull(result.NumberOfMembers);
            Assert.IsNotNull(result.ParentId);
            Assert.IsNotNull(result.Size);
            Assert.IsNotNull(result.Type);

        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var filename = "delete_test-" + Random.Next();
            var path = TestFolder + "/" + filename;
            var createdItem = await Client.Directory.Create(path, HomeId).ExecuteAsync();

            var sut = Client.Directory.Delete(path, HomeId);

            // Act
            await sut.ExecuteAsync();

            // Assert
            // Just pass if no exception thrown
        }

        [TestMethod]
        public async Task DeleteNonEmptyDirectoryNonRecursive()
        {
            // Arrange
            var filename = "delete_test-" + Random.Next();
            var path = TestFolder + "/" + filename;
            var createdItem1 = await Client.Directory.Create(path, HomeId).ExecuteAsync();

            using (var stream = new MemoryStream(CreateRandomBytes()))
            {
                var fileItem = await Client.File.Upload("test.bin", path, HomeId).ExecuteAsync(stream);
            }

            var sut = Client.Directory.Delete(path, HomeId, false);

            // Act
            try
            {
                await sut.ExecuteAsync();
            }

            // Assert
            catch (ServiceException ex)
            {
                //
                Assert.IsNotNull(ex.Error);
                Assert.AreEqual("409", ex.Error.Code);
                return;
            }

            Assert.Fail("Expected Exception was not thrown.");
        }

        [TestMethod]
        public async Task DeleteNonEmptyDirectoryRecursive()
        {
            // Arrange
            var filename = "delete_test-" + Random.Next();
            var path = TestFolder + "/" + filename;
            var createdItem1 = await Client.Directory.Create(path, HomeId).ExecuteAsync();

            using (var stream = new MemoryStream(CreateRandomBytes()))
            {
                var fileItem = await Client.File.Upload("test.bin", path, HomeId).ExecuteAsync(stream);
            }

            var sut = Client.Directory.Delete(path, HomeId, true);

            // Act
            await sut.ExecuteAsync();

            // Assert
            // Just pass if no exception thrown
        }

        [TestMethod]
        [Ignore]
        public async Task Copy()
        {
        }

        [TestMethod]
        [Ignore]
        public async Task Move()
        {
        }

        [TestMethod]
        [Ignore]
        public async Task Rename()
        {
        }

    }
}