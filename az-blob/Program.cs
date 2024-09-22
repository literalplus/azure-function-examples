using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Console.WriteLine("az-blob!\n");

ExampleAsync().GetAwaiter().GetResult();

Console.WriteLine("Done!");

static async Task ExampleAsync() {
    Console.WriteLine("Please enter the account key. Portal -> Storage account -> Security -> Access keys -> .. -> Key");
    var accountKey = Console.ReadLine();

    var connStr = "DefaultEndpointsProtocol=https;AccountName=learnaz204blob900124;AccountKey=" + accountKey;
    var client = new BlobServiceClient(connStr);

    var containerName = "cont-" + Guid.NewGuid().ToString();
    var containerClient = (await client.CreateBlobContainerAsync(containerName)).Value;
    Console.WriteLine("Container created: " + containerName + " - Press enter after the cloud minute has passed");
    Console.ReadLine();

    var fileName = "file-" + Guid.NewGuid().ToString() + ".txt";
    var path = Path.Combine("./data/", fileName);
    await File.WriteAllTextAsync(path, "henlo world");

    var blobClient = containerClient.GetBlobClient(fileName);
    Console.WriteLine("Target URL -> " + blobClient.Uri);

    using (var uploadStream = File.OpenRead(path)) {
        await blobClient.UploadAsync(uploadStream);
    }

    Console.WriteLine(" ... uploaded!");

    await foreach (var blobItem in containerClient.GetBlobsAsync()) {
        Console.WriteLine("\t" + blobItem.Name);
    }
    Console.WriteLine("Listing complete.");

    var downloadPath = path.Replace(".txt", ".downloaded.txt");
    var downloadInfo = (await blobClient.DownloadAsync()).Value;
    using (var downloadStream = File.OpenWrite(downloadPath)) {
        await downloadInfo.Content.CopyToAsync(downloadStream);
    }
    Console.WriteLine(" ... downloaded to: " + downloadPath);

    Console.WriteLine("Press enter to clean up (:");
    Console.ReadLine();

    await containerClient.DeleteAsync();
}