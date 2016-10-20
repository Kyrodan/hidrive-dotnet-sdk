using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public class ReceiveStreamRequest : BaseRequest, IReceiveStreamRequest
    {
        public ReceiveStreamRequest(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public Task<Stream> ExecuteAsync()
        {
            return this.ExecuteAsync(CancellationToken.None);
        }

        public async Task<Stream> ExecuteAsync(CancellationToken cancellationToken)
        {
            var retrievedStream = await this.SendStreamAsync(null, cancellationToken).ConfigureAwait(false);
            return retrievedStream;
        }
    }
}