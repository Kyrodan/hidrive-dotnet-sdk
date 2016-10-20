
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public interface IRequest
    {
        Task ExecuteAsync();
        Task ExecuteAsync(CancellationToken cancellationToken);
    }

    public interface IRequest<T>
    {
        Task<T> ExecuteAsync();
        Task<T> ExecuteAsync(CancellationToken cancellationToken);
    }
}