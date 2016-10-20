using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public interface IReceiveStreamRequest
    {
        Task<Stream> ExecuteAsync();
        Task<Stream> ExecuteAsync(CancellationToken cancellationToken);
    }

}