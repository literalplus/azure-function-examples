using System.Threading.Tasks;
using Microsoft.Identity.Client;

DoRun().GetAwaiter().GetResult();

static async Task DoRun() {
    Console.WriteLine("Please enter the client ID from the Portal, followed by the tenant ID");
    var clientId = Console.ReadLine();
    var tenantId = Console.ReadLine();
    
    var app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithRedirectUri("http://localhost")
            .Build();
    
    string[] scopes = { "user.read"};
    var authResult = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
    Console.WriteLine($"Token: {authResult.AccessToken}");
}
