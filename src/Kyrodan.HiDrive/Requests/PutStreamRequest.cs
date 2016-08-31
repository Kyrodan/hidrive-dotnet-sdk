using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    internal class PutStreamRequest<T> : BaseRequest, IPutStreamRequest<T>
    {
        public PutStreamRequest(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public async Task<T> ExecuteAsync(Stream content)
        {
            return await ExecuteAsync(content, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<T> ExecuteAsync(Stream content, CancellationToken cancellationToken)
        {
            this.Method = "PUT";
            var retrievedEntity = await this.SendAsync<T>(content, cancellationToken).ConfigureAwait(false);
            return retrievedEntity;
        }
    }
}