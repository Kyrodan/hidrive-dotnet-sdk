using System.Collections.Generic;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Requests
{
    internal class UserMeRequestBuilder : BaseRequestBuilder, IUserMeRequestBuilder
    {
        public UserMeRequestBuilder(string requestUrl, IBaseClient client)
            : base(requestUrl, client)
        {
        }

        public IGetRequest<User> Get(IEnumerable<string> fields = null)
        {
            var request = new GetRequest<User>(this.RequestUrl, this.Client);

            if (fields != null)
                request.QueryOptions.Add(new KeyValuePair<string, string>("fields", string.Join(",", fields)));

            return request;
        }
    }
}