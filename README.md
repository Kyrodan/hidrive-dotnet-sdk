# HiDrive .Net SDK
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Kyrodan/hidrive-dotnet-sdk/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Kyrodan.HiDriveSDK.svg?maxAge=2592000)](https://www.nuget.org/packages/Kyrodan.HiDriveSDK/)
[![Build status](https://ci.appveyor.com/api/projects/status/xfjn3pxoancr5l7t?svg=true)](https://ci.appveyor.com/project/Kyrodan/hidrive-dotnet-sdk)


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
var fields = new[] { User.Fields.Alias, User.Fields.Account, User.Fields.HomeId, User.Fields.Home };
var user = await client.User
                       .Me
                       .Get(fields)
                       .ExecuteAsync();
                       
var homeId = user.HomeId;
```

Get directory:
```csharp
var fields = new[] 
{ 
   DirectoryBaseItem.Fields.Path, 
   DirectoryBaseItem.Fields.Name, 
   DirectoryBaseItem.Fields.Type, 
   DirectoryBaseItem.Fields.Members, 
   DirectoryBaseItem.Fields.Members_Path, 
   DirectoryBaseItem.Fields.Members_Name, 
   DirectoryBaseItem.Fields.Members_Id, 
   DirectoryBaseItem.Fields.Members_Type, 
   DirectoryBaseItem.Fields.Members_IsReadable, 
   DirectoryBaseItem.Fields.Members_IsWritable, 
   DirectoryBaseItem.Fields.Members_ModiefiedDateTime 
};
var members = new []{ DirectoryMember.Directory, DirectoryMember.File };

var dir = await client.Directory
                      .Get("/some_path_inside_user_home", homeId, members, fields)
                      .ExecuteAsync();
```

Download a file:
```csharp
var stream = await client.File
                         .Download("/some_path_inside_user_home/myfile.txt", homeId)
                         .ExecuteAsync();
```

Upload new file or overwrite if it already exists:
```csharp
var stream = File.OpenRead(@"C:\Temp\myfile.txt");
var file = await client.File
                       .Upload("myfile.txt", "/some_path_inside_user_home", homeId)
                       .ExecuteAsync(stream);
```

## Ressources
* [Changelog](CHANGELOG.md)
* [NuGet Package](https://www.nuget.org/packages/Kyrodan.HiDriveSDK/)
* [HiDrive Developer Documentation](https://dev.strato.com/hidrive/)


## License

The source code is licensed under the [MIT license](https://github.com/Kyrodan/hidrive-dotnet-sdk/blob/master/LICENSE).


