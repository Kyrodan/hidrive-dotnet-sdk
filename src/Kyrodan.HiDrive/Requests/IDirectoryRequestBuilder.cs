using System.Collections.Generic;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    public interface IDirectoryRequestBuilder
    {
        IRequest<DirectoryItem> Get(string path = null, string pid = null, IEnumerable<DirectoryMember> members = null, IEnumerable<string> fields = null, int? offset = null, int? limit = null,
            string snapshot = null);

        IRequest<DirectoryItem> Create(string path = null, string pid = null);

        IRequest Delete(string path = null, string pid = null, bool? isRecursive = null);

        IRequest<DirectoryItem> GetHome(IEnumerable<DirectoryMember> members = null, IEnumerable<string> fields = null, int? offset = null, int? limit = null, string snapshot = null);

        IRequest<DirectoryItem> Copy(string sourcePath = null, string sourceId = null, string destPath = null, string destId = null, string snapshot = null);

        IRequest<DirectoryItem> Move(string sourcePath = null, string sourceId = null, string destPath = null, string destId = null);

        IRequest<DirectoryItem> Rename(string path = null, string pid = null, string name = null);
    }
}