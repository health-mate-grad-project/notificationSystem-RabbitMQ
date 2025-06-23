using FirebaseAdmin.Messaging;
using System;
using System.Threading.Tasks;

public class FcmService
{
    public async Task SendPushNotificationAsync(string deviceToken, ReminderMessage reminder)
    {
        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification()
            {
                Title = "Medication Reminder",
                Body = $"Time to take {reminder.dosage} of {reminder.medicationName}"
            }
        };

        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Console.WriteLine($"Sent notification: {response}");
    }
}
