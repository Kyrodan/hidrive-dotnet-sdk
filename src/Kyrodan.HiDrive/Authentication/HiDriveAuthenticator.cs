using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kyrodan.HiDrive.Authentication
{
    public class HiDriveAuthenticator : IHiDriveAuthenticator
    {
        public const string OAuthUrl = "https://www.hidrive.strato.com/oauth2";
        public const string AuthorizationUrl = OAuthUrl + "/authorize";
        public const string TokenUrl = OAuthUrl + "/token";


        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Func<HttpClientHandler> _httpClientHandlerFactory;

        public HiDriveAuthenticator(string clientId, string clientSecret, Func<HttpClientHandler> httpClientHandlerFactory = null)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException("clientId");
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException("clientSecret");

            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClientHandlerFactory = httpClientHandlerFactory != null ? httpClientHandlerFactory : () => new HttpClientHandler();
        }

        public OAuth2Token Token { get; private set; }

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException("request");

            // TODO
            request.Headers.Add("Authorization", (!string.IsNullOrEmpty(Token.TokenType) ? Token.TokenType : "Bearer")  + " " + Token.AccessToken);

            return Task.Delay(100);
        }

        public async Task<OAuth2Token> AuthenticateByRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException("refreshToken");

            Token = new OAuth2Token { RefreshToken = refreshToken };
            await GetAccessToken();

            return Token;
        }

        public async Task<OAuth2Token> AuthenticateByAuthorizationCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException("code");

            var httpClient = new HttpClient(_httpClientHandlerFactory());
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"client_id", _clientId},
                    {"client_secret", _clientSecret},
                    {"grant_type", "authorization_code"},
                    {"code", code},
                };

                var content = new FormUrlEncodedContent(parameters);
                var response = await httpClient.PostAsync(TokenUrl, content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException(responseString);

                Token = JsonConvert.DeserializeObject<OAuth2Token>(responseString);

                return Token;

            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public string GetAuthorizationCodeFromResponseUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");

            return GetAuthorizationCodeFromResponseUrl(new Uri(url));
        }

        public string GetAuthorizationCodeFromResponseUrl(Uri url)
        {
            if (url == null) throw new ArgumentNullException("url");

            string code = null;
            
            foreach (var pair in url.Query.TrimStart('?').Split('&'))
            {
                var elements = pair.Split('=');
                if (elements.Length != 2)
                {
                    continue;
                }

                switch (elements[0])
                {
                    case "code":
                        code = Uri.UnescapeDataString(elements[1]);
                        break;
                }
            }

            return code;
        }

        public string GetAuthorizationCodeRequestUrl(AuthorizationScope scope, string redirectUri = null)
        {
            var uriString =
                string.Format(
                    "{0}?client_id={1}&response_type=code&scope={2}",
                    AuthorizationUrl, _clientId, scope);

            if (!string.IsNullOrEmpty(redirectUri))
                uriString += "&redirect_uri=" + Uri.EscapeDataString(redirectUri);

            return uriString;
        }

        private async Task<string> GetAccessToken()
        {
            var client = new HttpClient(_httpClientHandlerFactory());

            var parameters = new Dictionary<string, string>
                {
                    {"client_id", _clientId},
                    {"client_secret", _clientSecret},
                    {"grant_type", "refresh_token"},
                    {"refresh_token", Token.RefreshToken},
                };

            var content = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync(TokenUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(responseString);

            Token = JsonConvert.DeserializeObject<OAuth2Token>(responseString);
            return Token.AccessToken;
        }

    }
}