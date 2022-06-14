using System;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;

class Program
{
    async static Task Main(string[] args)
    {
        try
        {
            await RunAsync();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }

    private static async Task RunAsync()
    {
        var credential = new ChainedTokenCredential(
            new ManagedIdentityCredential(),
            new AzureCliCredential()
        );

        var scopes = new string[] { "https://graph.microsoft.com/.default" }; 
        var client = new GraphServiceClient(credential, scopes);
        var result = await client.Users.Request().GetAsync();

        foreach(var user in result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(user.DisplayName);
            Console.ResetColor();
        }     
    }    
}

