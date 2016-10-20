using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    internal class SendStreamRequest<T> : BaseRequest, ISendStreamRequest<T>
    {
        public SendStreamRequest(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
            this.Method = "POST";
        }

        public async Task<T> ExecuteAsync(Stream content)
        {
            return await ExecuteAsync(content, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<T> ExecuteAsync(Stream content, CancellationToken cancellationToken)
        {
            var retrievedEntity = await this.SendAsync<T>(content, cancellationToken).ConfigureAwait(false);
            return retrievedEntity;
        }
    }
}