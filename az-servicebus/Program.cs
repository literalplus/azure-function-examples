using Azure.Messaging.ServiceBus;
using Azure.Identity;

DoRun().GetAwaiter().GetResult();

async Task DoRun() {
    var queueName = "az204q";
    var fqNamespace = "az204busvroom.servicebus.windows.net";
    await using var client = new ServiceBusClient(fqNamespace, new DefaultAzureCredential());
    await using var sender = client.CreateSender(queueName);

    using var messageBatch = await sender.CreateMessageBatchAsync();

    for (int i = 1; i <= 3; i++) {
        messageBatch.TryAddMessage(new ServiceBusMessage($"wow the {i}"));
    }

    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine("sent the things");

    await using var processor = client.CreateProcessor(queueName);

    processor.ProcessMessageAsync += HandleMessage;
    processor.ProcessErrorAsync += async _ => {};

    await processor.StartProcessingAsync();

    Console.WriteLine("Listening for 5s");
    await Task.Delay(5000);
    Console.WriteLine("Listened");

    await processor.StopProcessingAsync();
}

async Task HandleMessage(ProcessMessageEventArgs args) {
    var body = args.Message.Body.ToString();
    Console.WriteLine($" -> got: {body}");
    await args.CompleteMessageAsync(args.Message);
}