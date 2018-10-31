using System;
using System.Diagnostics;
using System.IO;
using Kyrodan.HiDrive.Authentication;

namespace Kyrodan.HiDrive.TestPreparation
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
            var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"microsoft\UserSecrets\kyrodan-hidrive-tests-f1cf20be-1d25-4501-a9f1-9dd9be27b7e7");

            Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, "secrets.json");
            File.Delete(filePath);

            using (var file = File.CreateText(filePath))
            {
                file.WriteLine("{");
                file.WriteLine($"  \"ClientSecret\": \"{clientId}\",");
                file.WriteLine($"  \"ClientId\": \"{clientSecret}\",");
                file.WriteLine($"  \"RefreshToken\": \"{refreshToken}\",");
                file.WriteLine("}");
            }
            Console.WriteLine("Successfully written ClientConfiguration.cs");
        }
    }
}
