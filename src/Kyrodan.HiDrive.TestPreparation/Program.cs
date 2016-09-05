using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kyrodan.HiDrive.Authentication;

namespace Kyrodan.HiDrive.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Client ID: ");
            var clientId = Console.ReadLine();

            Console.Write("Client Secret: ");
            var clientSecret = Console.ReadLine();

            Console.WriteLine("Please authenticate to HiDrive in your browser and come back here.");
            var authenticator = new HiDriveAuthenticator(clientId, clientSecret);
            var scope = new AuthorizationScope(AuthorizationRole.User, AuthorizationPermission.ReadWrite);
            var authUrl = authenticator.GetAuthorizationCodeRequestUrl(scope);
            Process.Start(authUrl);

            Console.Write("Code: ");
            var code = Console.ReadLine();

            Console.WriteLine("Gathering RefreshToken...");
            var token = authenticator.AuthenticateByAuthorizationCodeAsync(code);
            token.Wait();

            Console.WriteLine("RefreshToken: {0}", token.Result.RefreshToken);

            WriteClientConfiguration(clientId, clientSecret, token.Result.RefreshToken);

            Console.ReadLine();
        }

        private static void WriteClientConfiguration(string clientId, string clientSecret, string refreshToken)
        {
            var fileName = @"Kyrodan.HiDrive.Tests\ClientConfiguration.cs.tmpl";

            var inFiles = new[]
            {
                @"..\..\..\" + fileName,
                fileName,
                @"src\" + fileName,
            };

            string templateFile = null;

            foreach (var inFile in inFiles)
            {
                var f = Path.Combine(Environment.CurrentDirectory, inFile);
                if (File.Exists(f))
                {
                    templateFile = inFile;
                    break;
                }
            }

            if (templateFile == null)
            {
                Console.WriteLine("Faile to find template ClientConfiguration.cs.tmpl.");
                return;
            }

            var content = File.ReadAllText(templateFile);

            content = content.Replace("{{ClientId}}", clientId);
            content = content.Replace("{{ClientSecret}}", clientSecret);
            content = content.Replace("{{RefreshToken}}", refreshToken);

            var outFile = Path.ChangeExtension(templateFile, null);
            File.WriteAllText(outFile, content);
            Console.WriteLine("Successfully written ClientConfiguration.cs");
        }
    }
}
