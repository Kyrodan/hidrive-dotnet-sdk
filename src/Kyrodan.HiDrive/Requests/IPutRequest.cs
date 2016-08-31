using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public interface IPutRequest<T>
    {
        Task<T> ExecuteAsync();
        Task<T> ExecuteAsync(CancellationToken cancellationToken);
    }
}