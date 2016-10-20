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

            // Act
            var sut = Client.Directory.Get(null, HomeId);
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

            // Act
            var sut = Client.Directory.Get(null, HomeId, null, fields);
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
        public async Task GetHome()
        {
            // Arrange

            // Act
            var sut = Client.Directory.GetHome();
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Members);
            Assert.IsNotNull(result.Path);
        }

        [TestMethod]
        public async Task Create()
        {
            // Arrange
            var filename = "create_test-" + Random.Next();
            var path = TestFolder + "/" + filename;

            // Act
            var sut = Client.Directory.Create(path, HomeId);
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

            // Act
            var sut = Client.Directory.Delete(path, HomeId);
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

            // Act
            var sut = Client.Directory.Delete(path, HomeId, false);

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

            // Act
            var sut = Client.Directory.Delete(path, HomeId, true);
            await sut.ExecuteAsync();

            // Assert
            // Just pass if no exception thrown
        }

        [TestMethod]
        public async Task Copy()
        {
            // Arrange
            var sourcePath = TestFolder + "/copy_test-source-" + Random.Next();
            var source = await Client.Directory.Create(sourcePath, HomeId).ExecuteAsync();

            var destFolder = "copy_test-dest-" + Random.Next();
            var destPath = TestFolder + "/" + destFolder;

            // Act
            var sut = Client.Directory.Copy(sourcePath, HomeId, destPath, HomeId);
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
            Assert.AreEqual(destFolder, result.Name);
            Assert.IsNotNull(result.NameHash);
            Assert.IsNotNull(result.NumberOfMembers);
            Assert.IsNotNull(result.ParentId);
            Assert.IsNotNull(result.Size);
            Assert.IsNotNull(result.Type);
        }

        [TestMethod]
        public async Task Move()
        {
            // Arrange
            var sourcePath = TestFolder + "/move_test-source-" + Random.Next();
            var source = await Client.Directory.Create(sourcePath, HomeId).ExecuteAsync();

            var destFolder = "move_test-dest-" + Random.Next();
            var destPath = TestFolder + "/" + destFolder;

            // Act
            var sut = Client.Directory.Move(sourcePath, HomeId, destPath, HomeId);
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CHash);
            Assert.AreEqual(source.CreatedDateTime, result.CreatedDateTime);
            Assert.AreEqual(source.HasDirectories, result.HasDirectories);
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.IsReadable, result.IsReadable);
            Assert.AreEqual(source.IsWritable, result.IsWritable);
            Assert.IsNotNull(result.MetaHash);
            Assert.IsNotNull(result.MetaOnlyHash);
            Assert.AreEqual(source.ModifiedDateTime, result.ModifiedDateTime);
            Assert.AreEqual(destFolder, result.Name);
            Assert.IsNotNull(result.NameHash);
            Assert.AreEqual(source.NumberOfMembers, result.NumberOfMembers);
            Assert.IsNotNull(result.ParentId);
            Assert.AreEqual(source.Size, result.Size);
            Assert.AreEqual(source.Type, result.Type);
        }

        [TestMethod]
        public async Task Rename()
        {
            // Arrange
            var sourcePath = TestFolder + "/rename_test-source-" + Random.Next();
            var source = await Client.Directory.Create(sourcePath, HomeId).ExecuteAsync();

            var destName = "rename_test-dest-" + Random.Next();

            // Act
            var sut = Client.Directory.Rename(sourcePath, HomeId, destName);
            var result = await sut.ExecuteAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Path);
            Assert.IsNotNull(result.CHash);
            Assert.AreEqual(source.CreatedDateTime, result.CreatedDateTime);
            Assert.AreEqual(source.HasDirectories, result.HasDirectories);
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.IsReadable, result.IsReadable);
            Assert.AreEqual(source.IsWritable, result.IsWritable);
            Assert.IsNotNull(result.MetaHash);
            Assert.IsNotNull(result.MetaOnlyHash);
            Assert.AreEqual(source.ModifiedDateTime, result.ModifiedDateTime);
            Assert.AreEqual(destName, result.Name);
            Assert.IsNotNull(result.NameHash);
            Assert.AreEqual(source.NumberOfMembers, result.NumberOfMembers);
            Assert.AreEqual(source.ParentId, result.ParentId);
            Assert.AreEqual(source.Size, result.Size);
            Assert.AreEqual(source.Type, result.Type);
        }

    }
}