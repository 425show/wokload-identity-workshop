require("isomorphic-fetch");
const {Client} = require('@microsoft/microsoft-graph-client');
const { TokenCredentialAuthenticationProvider } = require("@microsoft/microsoft-graph-client/authProviders/azureTokenCredentials");
const { ChainedTokenCredential, AzureCliCredential, ManagedIdentityCredential } = require("@azure/identity");

async function main() {
    console.log("Retrieving data from MS Graph...");

    const creds = new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential());
    const authProvider = new TokenCredentialAuthenticationProvider(creds, { scopes: ["https://graph.microsoft.com/.default"] });
    const client = new Client.initWithMiddleware({
        debugLogging:true,
        authProvider: authProvider
    });

    await client
        .api("/users/")
        .get()
        .then((res) => {
            res.value.forEach(user => {
                console.info(user.displayName);
            });
        });
};

main();