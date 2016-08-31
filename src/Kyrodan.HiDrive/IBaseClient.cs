using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive
{
    public interface IBaseClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task AuthenticateRequestAsync(HttpRequestMessage request);
    }
}