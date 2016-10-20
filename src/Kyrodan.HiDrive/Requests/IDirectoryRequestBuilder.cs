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
    }
}