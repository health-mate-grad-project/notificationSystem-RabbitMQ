using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class ReminderConsumer
{
    private readonly FcmService _fcmService = new FcmService();
    private readonly FcmTokenFetcher _tokenFetcher = new FcmTokenFetcher();

    public void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "reminder_queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var reminder = JsonSerializer.Deserialize<ReminderMessage>(json);

                if (reminder == null)
                {
                    Console.WriteLine("[!] Deserialization failed.");
                    return;
                }

                string? token = await _tokenFetcher.GetTokenForPatientAsync(reminder.patientId);
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine($"[!] No FCM token for patient {reminder.patientId}");
                    return;
                }

                await _fcmService.SendPushNotificationAsync(token, reminder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error processing reminder: {ex.Message}");
            }
        };

        channel.BasicConsume(queue: "reminder_queue", autoAck: true, consumer: consumer);
        Console.WriteLine("[*] Waiting for reminders...");
    }
}
