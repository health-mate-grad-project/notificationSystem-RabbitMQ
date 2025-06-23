using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

class Program
{
    static void Main(string[] args)
    {
        // Initialize Firebase Admin SDK once at startup
        InitializeFirebase();

        var consumer = new ReminderConsumer();
        consumer.StartListening();

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }

    private static void InitializeFirebase()
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("healthmate-82413-firebase-adminsdk-fbsvc-a0f76560f8.json"),
        });

        Console.WriteLine("Firebase Admin SDK initialized.");
    }
}
