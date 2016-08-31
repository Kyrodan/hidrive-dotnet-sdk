using System.Threading;
using System.Threading.Tasks;

namespace Kyrodan.HiDrive.Requests
{
    public class GetRequest<T> : BaseRequest, IGetRequest<T>
    {
        public GetRequest(string requestUrl, IBaseClient client) 
            : base(requestUrl, client)
        {
        }

        public Task<T> ExecuteAsync()
        {
            return this.ExecuteAsync(CancellationToken.None);
        }

        public async Task<T> ExecuteAsync(CancellationToken cancellationToken)
        {
            this.Method = "GET";
            var retrievedEntity = await this.SendAsync<T>(null, cancellationToken).ConfigureAwait(false);
            //this.InitializeCollectionProperties(retrievedEntity);
            return retrievedEntity;
        }

    }
}