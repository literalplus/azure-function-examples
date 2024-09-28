using Azure.Identity;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;

RunAsync().GetAwaiter().GetResult();

static async Task RunAsync() {
    Console.WriteLine("henlo from az-keyvault");
    var client = new SecretClient(new Uri("https://learnvault3245634526.vault.azure.net/"), new DefaultAzureCredential());
    var secret = await client.GetSecretAsync("examplepw");
    Console.WriteLine($"hacker voice im in -> {secret}");
}