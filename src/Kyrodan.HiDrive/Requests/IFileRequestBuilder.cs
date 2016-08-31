using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    public interface IFileRequestBuilder
    {
        //IFileRequest Request();
        //IFileRequest Request(IEnumerable<KeyValuePair<string, string>> additionalParameters);


        IGetStreamRequest Download(string path = null, string pid = null, string snapshot = null);
        //string UploadNew(string dir = null, string dir_id = null, string name = null);
        IPutStreamRequest<FileItem> Upload(string name, string dir = null, string dir_id = null);
        //string UploadPart(string path = null, string pid = null, long offset = 0);

        //string Delete(string path = null, string pid = null);
    }
}