---
page_type: sample
languages:
- javascript
products:
- nodejs
- ms-graph
- azure-active-directory
description: "Demonstrates how to use the Microsoft Graph SDK with the Azure.Identity in a console application using the application's own identity (client credentials flow)"
urlFragment: "ms-identity-javascript-nodejs-console"
---

# A Node.js console application calling Microsoft Graph with a Service Principal

## About this sample

### Overview

   This sample application shows how to use the [Microsoft identity platform endpoint](http://aka.ms/aadv2) to access Microsoft Graph data in a long-running, non-interactive process.  It uses the [Azure.Identity](https://github.com/Azure/azure-sdk-for-js/tree/main/sdk/identity/identity) to acquire an access token, which can be used to call the [Microsoft Graph](https://graph.microsoft.io) and access organizational data.

   The app is a Node.js Console application. It gets the list of users in an Azure AD tenant by using the `Microsoft Graph JavaScript` library.

### Run the sample

1. On the command line, navigate to the root of the repository, and type `npm install`.
1. On the command line, navigate to the root of the repository and run the sample application with `node index.js`.

Running the application should retrieve and display all the users in the tenant.

## About the code

The relevant code for this sample is in the `index.js` file, which is the only file after all :) The code is minimalist and succint all contained in the `Main()` method. The steps are:

1. Create a Token Credential.

   The Azure.Identity library offers multiple ways for authenticating to Azure. In this instance we'll use the `ChainedTokenCredential` class, which is a wrapper around a `ManagedIdentityCredential` and `AzureCliCredential`. The code will first attempt to authenticate using the `ManagedIdentityCredential` and if that fails, it will fall back to the `AzureCliCredential`. This way the code is production ready!
   
   As for the **Scopes**, due to the fact that this is a daemon app and there is no user to interactively consent to the scopes, all scopes have been statically declared during the application registration step. In addition, all scopes should have been admin-consented. Therefore the only possible scope is **resource/.default** (here `https://graph.microsoft.com/.default`) which means --> *all the static permissions defined in the application*.

```JavaScript
const creds = new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential());
const authProvider = new TokenCredentialAuthenticationProvider(creds, { scopes: ["https://graph.microsoft.com/.default"] });

```

2. Instantiate the Graph client.

```JavaScript
const client = new Client.initWithMiddleware({
      debugLogging:true,
      authProvider: authProvider
});
```

3. Call the Graph API to retrieve all the users in the tenant.

```javaScript
await client
      .api("/users/")
      .get()
      .then((res) => {
         res.value.forEach(user => {
               console.info(user.displayName);
         });
      });

```

## Troubleshooting

### Did you forget to provide admin consent? This is needed for daemon apps

If you get an error when calling the API `Insufficient privileges to complete the operation.`, this is because the tenant administrator has not granted permissions
to the application.

You will typically see, on the output window, something like the following:

```Json
Failed to call the Web Api: Forbidden
Content: {
  "error": {
    "code": "Authorization_RequestDenied",
    "message": "Insufficient privileges to complete the operation.",
    "innerError": {
      "request-id": "<a guid>",
      "date": "<date>"
    }
  }
}
```





