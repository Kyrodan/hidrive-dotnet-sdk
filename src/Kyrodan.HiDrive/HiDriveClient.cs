using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Authentication;
using Kyrodan.HiDrive.Requests;

namespace Kyrodan.HiDrive
{
    public class HiDriveClient : IHiDriveClient, IBaseClient
    {
        public const string ApiUrl = "https://api.hidrive.strato.com/2.1";

        private readonly IHiDriveAuthenticator _authenticator;
        private readonly Func<HttpClientHandler> _httpClientHandlerFactory;


        public HiDriveClient(IHiDriveAuthenticator authenticator , Func<HttpClientHandler> httpClientHandlerFactory = null)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");

            _authenticator = authenticator;
            _httpClientHandlerFactory = httpClientHandlerFactory != null ? httpClientHandlerFactory : () => new HttpClientHandler();
        }

        private HttpClient CreateClient()
        {
            var clientHandler = _httpClientHandlerFactory();
            var client = new HttpClient(clientHandler);
            return client;
        }


        public IDirectoryRequestBuilder Directory
        {
            get
            {
                return new DirectoryRequestBuilder(ApiUrl + "/dir", this);
            }
        }

        public IFileRequestBuilder File {
            get
            {
                return new FileRequestBuilder(ApiUrl + "/file", this);
            }
        }

        public IUserRequestBuilder User
        {
            get
            {
                return new UserRequestBuilder(ApiUrl + "/user", this);
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await this.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            using (var client = CreateClient())
            {
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

                return await client.SendAsync(request, completionOption, cancellationToken);
            }
        }

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            return _authenticator.AuthenticateRequestAsync(request);
        }
    }
}