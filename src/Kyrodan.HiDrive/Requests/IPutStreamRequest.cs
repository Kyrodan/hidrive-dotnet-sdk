using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public interface IPutStreamRequest<T>
    {
        Task<T> ExecuteAsync(Stream content);
        Task<T> ExecuteAsync(Stream content, CancellationToken cancellationToken);
    }
}