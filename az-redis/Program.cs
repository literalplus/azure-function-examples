using StackExchange.Redis;
using Newtonsoft.Json;

DoRun().GetAwaiter().GetResult();

async Task DoRun() {
    Console.WriteLine("Enter Redis pw pls");
    var pw = Console.ReadLine();
    var connStr = $"az204cachehorse.redis.cache.windows.net:6380,password={pw},ssl=True,abortConnect=False";

    using var mx = await ConnectionMultiplexer.ConnectAsync(connStr);
    var db = mx.GetDatabase();
    await db.StringSetAsync("horse:color", "purple");

    var readback = await db.StringGetAsync("horse:color");
    Console.WriteLine($"peter the horse is {readback}");

    var horse = new Horse() {
        Id = "4gscdfghd6",
        Name = "Little Sebastian",
    };
    string ser = Newtonsoft.Json.JsonConvert.SerializeObject(horse);
    await db.StringSetAsync("horse:best", ser);

    var rb2 = await db.StringGetAsync("horse:best");
    var h2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Horse>(rb2.ToString());
    Console.WriteLine($"The horse is now {h2}");
}

public class Horse {
    public string? Id { get; set; }
    public string? Name { get; set; }

    public override string ToString() {
        return $"{Id} named {Name}";
    }
}