using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;


Console.WriteLine("az-cosmos!\n");

ExampleAsync().GetAwaiter().GetResult();

static async Task ExampleAsync() {
    Console.WriteLine("Enter connection string from Portal:");
    var connStr = Console.ReadLine();
    var client = new CosmosClient(connStr, new CosmosClientOptions());
    var db = (await client.CreateDatabaseIfNotExistsAsync(id: "ToDoList")).Database;
    var container = (await db.CreateContainerIfNotExistsAsync(id: "Items", partitionKeyPath: "/category")).Container;

    var dummy = new ToDoListItem() {
        Id = Guid.NewGuid(),
        Category = "personal",
        Name = "Delete example item from Cosmos DB",
    };
    await container.CreateItemAsync(dummy, dummy.GetPartitionKey());

    var readItemBack = await container.ReadItemAsync<ToDoListItem>(dummy.Id.ToString(), dummy.GetPartitionKey());
    Console.WriteLine($"Read: {readItemBack.Resource.Name}");

    Console.WriteLine("Press enter to delete it hehe.");
    Console.ReadLine();
    await container.DeleteItemAsync<ToDoListItem>(dummy.Id.ToString(), dummy.GetPartitionKey());
}

public class ToDoListItem {
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "category")]
    public string Category { get; set; } = default!;

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = default!;

    public PartitionKey GetPartitionKey() {
        return new PartitionKey(this.Category);
    }
}