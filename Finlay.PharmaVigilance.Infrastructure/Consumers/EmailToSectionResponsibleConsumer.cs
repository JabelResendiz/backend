// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Domain.Events;
// using Microsoft.Extensions.Hosting;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;
// using System.Text.Json;

// namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

// public class EmailToSectionResponsibleConsumer : BackgroundService
// {
//     private readonly IEmailService _emailService;

//     public EmailToSectionResponsibleConsumer(IEmailService emailService)
//     {
//         _emailService = emailService;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         var factory = new ConnectionFactory()
//         {
//             HostName = "localhost",
//             UserName = "guest",
//             Password = "guest"
//         };

//         var connection = await factory.CreateConnectionAsync();
//         var channel = await connection.CreateChannelAsync();

//         var queueName = "EmailToSectionResponsibleEvent";

//         await channel.QueueDeclareAsync(queueName, true, false, false);

//         var consumer = new AsyncEventingBasicConsumer(channel);

//         consumer.ReceivedAsync += async (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);

//             var data = JsonSerializer.Deserialize<EmailToSectionResponsibleEvent>(message);

//             int retryCount = 0;

//             if (ea.BasicProperties.Headers != null &&
//                 ea.BasicProperties.Headers.ContainsKey("retry-count"))
//             {
//                 retryCount = Convert.ToInt32(ea.BasicProperties.Headers["retry-count"]);
//             }

//             try
//             {
//                 await _emailService.SendEmailAsync(
//                     data!.SectionResponsibleEmail,
//                     "Nuevo Reporte",
//                     $"Se registró el reporte {data.ReportNumber}."
//                 );

//                 await channel.BasicAckAsync(ea.DeliveryTag, false);
//             }
//             catch
//             {

//                 retryCount++;

//                 var delay = Math.Pow(2, retryCount);
//                 var delayMs = (int)(delay * 1000);

//                 Console.WriteLine($"⏳ Retry {retryCount} en {delayMs} ms");

//                 await Task.Delay(delayMs);

//                 var props = new BasicProperties
//                 {
//                     Headers = new Dictionary<string, object?>
//                     {
//                         { "retry-count", retryCount }
//                     }
//                 };

//                 await channel.BasicPublishAsync(
//                     exchange: "",
//                     routingKey: queueName,
//                     mandatory: false,
//                     basicProperties: props,
//                     body: ea.Body.ToArray()
//                 );

//                 await channel.BasicAckAsync(ea.DeliveryTag, false);

//             }

//         };

//         await channel.BasicConsumeAsync(queueName, false, consumer);

//         await Task.Delay(Timeout.Infinite, stoppingToken);
//     }


// }