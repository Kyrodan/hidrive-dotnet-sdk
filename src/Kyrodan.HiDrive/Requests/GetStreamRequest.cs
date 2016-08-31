using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public class GetStreamRequest : BaseRequest, IGetStreamRequest
    {
        public GetStreamRequest(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public Task<Stream> ExecuteAsync()
        {
            return this.ExecuteAsync(CancellationToken.None);
        }

        public async Task<Stream> ExecuteAsync(CancellationToken cancellationToken)
        {
            this.Method = "GET";
            var retrievedStream = await this.SendStreamAsync(null, cancellationToken).ConfigureAwait(false);
            return retrievedStream;
        }
    }
}