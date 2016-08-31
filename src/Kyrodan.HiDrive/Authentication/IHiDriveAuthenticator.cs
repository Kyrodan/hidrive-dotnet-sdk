using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Models;

namespace Kyrodan.HiDrive.Authentication
{
    public interface IHiDriveAuthenticator
    {
        OAuth2Token Token { get; }

        Task AuthenticateRequestAsync(HttpRequestMessage request);

        Task<OAuth2Token> AuthenticateByRefreshTokenAsync(string refreshToken);

        Task<OAuth2Token> AuthenticateByAuthorizationCodeAsync(string code);

        string GetAuthorizationCodeFromResponseUrl(string url);

        string GetAuthorizationCodeFromResponseUrl(Uri url);

        string GetAuthorizationCodeRequestUrl(AuthorizationScope scope, string redirectUri = null);
    }
}