# HiDrive .Net SDK


## Introduction

The [HiDrive .Net SDK](https://github.com/Kyrodan/hidrive-dotnet-sdk) lets you easily integrate HiDrive into your C# App.
It wraps the [HiDrive REST API](https://dev.strato.com/hidrive/).

The HiDrive SDK is built as a Portable Class Library and targets the following frameworks: 

* .NET 4.5.1 
* .NET for Windows Store apps 
* Windows Phone 8.1 and higher
 

## Installation

To install the HiDrive SDK via NuGet

* Search for `Kyrodan.HiDriveSDK` in the NuGet Library, or
* Type `Install-Package Kyrodan.HiDriveSDK` into the Package Manager Console.


## Getting Started

### 1. Register your app

Register your app [here](https://dev.strato.com/hidrive/get_key) to get an API Key. 
Please choose the right API Key type and Redirect URI. For detailed information see [here](https://dev.strato.com/hidrive/content.php?r=150--OAuth2-Authentication).

### 2. Authorizing your app for your HiDrive account
```csharp
var authenticator = new HiDriveAuthenticator("appId", "appSecret");
var authorizationUrl = authenticator.GetAuthorizationCodeRequestUrl(new AuthorizationScope(AuthorizationRole.User, AuthorizationPermission.ReadWrite));

var code = YourAuthorizationUi.GetCode(authorizationUrl);
// Alternative:
// Process.Start(authorizationUrl)
//
// Helper to get code from URL:
// authenticator.GetAuthorizationCodeFromResponseUrl(redirectedUrl)

var token = await authenticator.AuthenticateByAuthorizationCodeAsync(code);
```
Now you have a valid token. 
Store `toke.RefreshToken` to create an authenticated HiDriveClient later.

### 3. Create authenticated HiDriveClient object

If you have a Refresh Token you can authenticate to HiDrive and create a `HiDriveClient` object.

```csharp
var authenticator = new HiDriveAuthenticator("appId", "appSecret");
authenticator.AuthenticateByRefreshTokenAsync(refreshToken);

var client = new HiDriveClient(authenticator);
```

### 4. Make requests

The `HiDrive .Net SDK` follows the documented API endpoints - see [API-Refrence](https://dev.strato.com/hidrive/content.php?r=115-reference).

Get current user data:
```csharp
var user = await client.User
                       .Me
                       .Get()
                       .ExecuteAsync();
```

Get directory:
```csharp
var dir = await client.Directory
                      .Get("/home/useralias", null, new [] {DirectoryMember.Directory, DirectoryMember.File })
                      .ExecuteAsync();
```

Download a file
```csharp
var stream = await client.File
                         .Download("/home/useralias/myfile.txt")
                         .ExecuteAsync();
```

Upload new file or overwrite if it already exists
```csharp
var stream = File.OpenRead(@"C:\Temp\myfile.txt");
var file = await client.File
                       .Upload("myfile.txt", "/home/useralias")
                       .ExecuteAsync(stream);
```

## License

The source code is licensed under the [MIT license](https://github.com/Kyrodan/hidrive-dotnet-sdk/blob/master/LICENSE).


