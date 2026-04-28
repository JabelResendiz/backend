// using System.Text;
// using System.Text.Json;
// using Finlay.PharmaVigilance.Application.Interfaces;
// using RabbitMQ.Client;

// namespace Finlay.PharmaVigilance.Infrastructure.Messaging;

// public class RabbitMqEventBus : IEventBus
// {
//     private readonly ConnectionFactory _factory;

//     public RabbitMqEventBus()
//     {
//         _factory = new ConnectionFactory()
//         {
//             HostName = "localhost",
//             UserName = "guest",
//             Password = "guest"
//         };
//     }

//     public async Task PublishAsync<T>(T @event)
//     {
//         using var connection = await _factory.CreateConnectionAsync();
//         using var channel = await connection.CreateChannelAsync();

//         var queueName = typeof(T).Name;

//         await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

//         var message = JsonSerializer.Serialize(@event);
//         var body = Encoding.UTF8.GetBytes(message);

//         await channel.BasicPublishAsync("", queueName, body);

//         Console.WriteLine($"[x] Event sent: {queueName}");
//     }
// }