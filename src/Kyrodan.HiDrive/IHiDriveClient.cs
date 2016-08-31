using Kyrodan.HiDrive.Requests;

namespace Kyrodan.HiDrive
{
    public interface IHiDriveClient
    {
        IDirectoryRequestBuilder Directory { get; }

        IFileRequestBuilder File { get; }

        IUserRequestBuilder User { get; }
    }
}