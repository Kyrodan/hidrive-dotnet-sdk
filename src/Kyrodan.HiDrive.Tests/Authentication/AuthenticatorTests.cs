using System.Net.Http;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kyrodan.HiDrive.Tests.Authentication
{
    [TestClass]
    public class AuthenticatorTests
    {
        public static IConfiguration Configuration { get; set; }

        static AuthenticatorTests()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<AuthenticatorTests>();

            Configuration = builder.Build();
        }

        [TestMethod]
        public void GetAuthorizationCodeRequestUrl()
        {
            var sut = GetValidAuthenticator();

            var result =
                sut.GetAuthorizationCodeRequestUrl(new AuthorizationScope(AuthorizationRole.User,
                    AuthorizationPermission.ReadWrite));

            var expected = $"https://www.hidrive.strato.com/oauth2/authorize?client_id={Configuration["ClientId"]}&response_type=code&scope=user,rw";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetAuthorizationCodeRequestUrlWithRedirectUri()
        {
            var sut = GetValidAuthenticator();

            var result =
                sut.GetAuthorizationCodeRequestUrl(new AuthorizationScope(AuthorizationRole.User,
                    AuthorizationPermission.ReadWrite), "http://localhost");

            var expected = $"https://www.hidrive.strato.com/oauth2/authorize?client_id={Configuration["ClientId"]}&response_type=code&scope=user,rw&redirect_uri=http%3A%2F%2Flocalhost";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetAuthorizationCodeFromResponseUrl()
        {
            var sut = GetValidAuthenticator();

            var result = sut.GetAuthorizationCodeFromResponseUrl("http://localhost/?code=mycode");

            Assert.AreEqual("mycode", result);
        }

        [TestMethod]
        public async Task AuthenticateByRefreshTokenWithValidParameters()
        {
            var sut = GetValidAuthenticator();

            var result = await sut.AuthenticateByRefreshTokenAsync(Configuration["RefreshToken"]);

            Assert.IsNotNull(result);
            Assert.AreEqual("Bearer", result.TokenType);
            Assert.AreEqual(Configuration["RefreshToken"], result.RefreshToken);
            Assert.IsNotNull(result.AccessToken);
            Assert.IsNotNull(result.ExpiresIn);
        }

        [TestMethod]
        public async Task AuthenticateByRefreshTokenWithInvalidToken()
        {
            var sut = GetValidAuthenticator();

            try
            {
                var result = await sut.AuthenticateByRefreshTokenAsync("invalid_token");
                Assert.Fail("AuthenticationException expected");
            }
            catch (AuthenticationException ex)
            {
                var error = ex.Error;

                Assert.IsNotNull(error);
                Assert.AreEqual("invalid_request", error.Error);
                Assert.AreEqual("invalid refresh_token (format)", error.Description);
            }
        }

        [TestMethod]
        public async Task AuthenticateByRefreshTokenWithInvalidClientId()
        {
            var sut = new HiDriveAuthenticator("invalid_id", Configuration["ClientSecret"]);

            try
            {
                var result = await sut.AuthenticateByRefreshTokenAsync(Configuration["RefreshToken"]);
                Assert.Fail("AuthenticationException expected");
            }
            catch (AuthenticationException ex)
            {
                var error = ex.Error;

                Assert.IsNotNull(error);
                Assert.AreEqual("invalid_request", error.Error);
                Assert.AreEqual("invalid client_id (param auth)", error.Description);
            }
        }

        [TestMethod]
        public async Task AuthenticateByRefreshTokenWithInvalidClientSecret()
        {
            var sut = new HiDriveAuthenticator(Configuration["ClientId"], "invalid_secret");

            try
            {
                var result = await sut.AuthenticateByRefreshTokenAsync(Configuration["RefreshToken"]);
                Assert.Fail("AuthenticationException expected");
            }
            catch (AuthenticationException ex)
            {
                var error = ex.Error;

                Assert.IsNotNull(error);
                Assert.AreEqual("invalid_request", error.Error);
                Assert.AreEqual("invalid client_secret (param auth)", error.Description);
            }
        }

        [TestMethod]
        public async Task AuthenticateRequestWithoutBeingAuthenticated()
        {
            var sut = GetValidAuthenticator();

            var message = new HttpRequestMessage();

            try
            {
                await sut.AuthenticateRequestAsync(message);
                Assert.Fail("AuthenticationException expected");
            }
            catch (AuthenticationException ex)
            {
                var error = ex.Error;

                Assert.IsNotNull(error);
                Assert.AreEqual("no_token", error.Error);
                Assert.AreEqual("No token provided. Please authenticate first.", error.Description);
            }
        }

        [TestMethod]
        public async Task AuthenticateRequestWithBeingAuthenticated()
        {
            var sut = GetValidAuthenticator();
            var token = await sut.AuthenticateByRefreshTokenAsync(Configuration["RefreshToken"]);

            var message = new HttpRequestMessage();
            await sut.AuthenticateRequestAsync(message);

            Assert.AreEqual("Bearer", message.Headers.Authorization.Scheme);
            Assert.AreEqual(token.AccessToken, message.Headers.Authorization.Parameter);
        }


        protected IHiDriveAuthenticator GetValidAuthenticator()
        {
            return new HiDriveAuthenticator(Configuration["ClientId"], Configuration["ClientSecret"]);
        }
    }
}
