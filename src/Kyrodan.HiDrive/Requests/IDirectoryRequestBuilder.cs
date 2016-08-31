using System.Collections.Generic;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    public interface IDirectoryRequestBuilder
    {
        IGetRequest<DirectoryItem> Get(string path = null, string pid = null, IEnumerable<DirectoryMember> members = null, IEnumerable<string> fields = null, int? offset = null, int? limit = null,
            string snapshot = null);
    }
}