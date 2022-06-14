---
topic: sample
languages:
  - csharp
  - azurepowershell
products:
  - azure-active-directory
  - dotnet-core
  - office-ms-graph
description: "Shows how a daemon console app uses a Service Principal with the Azure.Identity library to get an access token and call Microsoft Graph."
---

# A .NET Core 6.0 simple daemon console application calling Microsoft Graph with a Service Principal.

## About this sample

### Overview

  This sample application shows how to use the [Microsoft identity platform endpoint](http://aka.ms/aadv2) to access Microsoft Graph data in a long-running, non-interactive process.  It uses the [Azure.Identity](https://docs.microsoft.com/en-us/dotnet/api/azure.identity?view=azure-dotnet) to acquire an access token, which can be used to call the [Microsoft Graph](https://graph.microsoft.io) and access organizational data

  The app is a .NET Core 6.0 Console application. It gets the list of users in an Azure AD tenant by using the `Microsoft Graph .NET` library.

### Run the sample

  Clean the solution, rebuild the solution, and run it.

  ```CSharp
  dotnet clean
  dotnet build
  dotnet run
  ```
  Running the application should retrieve and display all the users in the tenant.

## About the code

  The relevant code for this sample is in the `Program.cs` file, in the `RunAsync()` method. The steps are:

1. Create a Token Credential.

   The Azure.Identity library offers multiple ways for authenticating to Azure. In this instance we'll use the `ChainedTokenCredential` class, which is a wrapper around a `ManagedIdentityCredential` and `AzureCliCredential`. The code will first attempt to authenticate using the `ManagedIdentityCredential` and if that fails, it will fall back to the `AzureCliCredential`. This way the code is production ready!

```CSharp
var credential = new ChainedTokenCredential(
  new ManagedIdentityCredential(),
  new AzureCliCredential()
);
```

2. Define the scopes.

   Due to the fact that this is a service and there is no user to interactively consent to the scopes, all scopes have been statically declared during the application registration step. And all scopes should have been admin-consented. Therefore the only possible scope is **resource/.default** (here `https://graph.microsoft.com/.default`) which means "all the static permissions defined in the application".

```CSharp
var scopes = new string[] { "https://graph.microsoft.com/.default" };
```

3. Instantiate the Graph client and call the Graph API.

```CSharp
var client = new GraphServiceClient(credential, scopes);
```

4. Call MS Graph to retrieve all the users in the tenant
```CSharp
var result = await client.Users.Request().GetAsync();
```