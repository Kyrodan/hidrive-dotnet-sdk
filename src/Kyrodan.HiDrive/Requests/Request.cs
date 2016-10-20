using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public class Request<T> : BaseRequest, IRequest<T>
    {
        public Request(string requestUrl, IBaseClient client) 
            : base(requestUrl, client)
        {
        }

        public Task<T> ExecuteAsync()
        {
            return this.ExecuteAsync(CancellationToken.None);
        }

        public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
        {
            var retrievedEntity = await this.SendAsync<T>(null, cancellationToken).ConfigureAwait(false);
            //this.InitializeCollectionProperties(retrievedEntity);
            return retrievedEntity;
        }

    }

    public class Request : BaseRequest, IRequest
    {
        public Request(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public Task ExecuteAsync()
        {
            return this.ExecuteAsync(CancellationToken.None);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await this.SendAsync(null, cancellationToken).ConfigureAwait(false);
            //this.InitializeCollectionProperties(retrievedEntity);
        }

    }

}