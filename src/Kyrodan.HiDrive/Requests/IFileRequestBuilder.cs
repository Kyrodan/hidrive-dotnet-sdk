using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    public interface IFileRequestBuilder
    {
        //IFileRequest Request();
        //IFileRequest Request(IEnumerable<KeyValuePair<string, string>> additionalParameters);

        IReceiveStreamRequest Download(string path = null, string pid = null, string snapshot = null);

        ISendStreamRequest<FileItem> Upload(string name, string dir = null, string dir_id = null, UploadMode mode = UploadMode.CreateOnly);

        IRequest Delete(string path = null, string pid = null);

        IRequest<FileItem> Copy(string sourcePath = null, string sourceId = null, string destPath = null, string destId = null, string snapshot = null);

        IRequest<FileItem> Rename(string path = null, string pid = null, string name = null);

        IRequest<FileItem> Move(string sourcePath = null, string sourceId = null, string destPath = null, string destId = null);
    }
}