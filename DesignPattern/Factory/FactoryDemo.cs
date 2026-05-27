namespace DesignPattern.Factory;

public static class FactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("   FACTORY METHOD PATTERN - Notification Demo ");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        Console.WriteLine("-- Step 1: Creating notification creators --");
        Console.WriteLine();

        NotificationCreator emailCreator = new EmailNotificationCreator();
        NotificationCreator smsCreator = new SmsNotificationCreator();
        NotificationCreator pushCreator = new PushNotificationCreator();

        Console.WriteLine("   Created: EmailNotificationCreator");
        Console.WriteLine("   Created: SmsNotificationCreator");
        Console.WriteLine("   Created: PushNotificationCreator");
        Console.WriteLine();

        Console.WriteLine("-- Step 2: Sending via Email --");
        Console.WriteLine();
        emailCreator.SendNotification("john@example.com", "Your order #1234 has shipped!");
        Console.WriteLine();

        Console.WriteLine("-- Step 3: Sending via SMS --");
        Console.WriteLine();
        smsCreator.SendNotification("+84-912-345-678", "Your OTP code is 482917. Valid for 5 minutes.");
        Console.WriteLine();

        Console.WriteLine("-- Step 4: Sending via Push --");
        Console.WriteLine();
        pushCreator.SendNotification("user_john_doe", "Flash sale! 50% off all items for the next 2 hours!");
        Console.WriteLine();

        Console.WriteLine("-- Step 5: Polymorphic batch sending --");
        Console.WriteLine();
        Console.WriteLine("   Sending the same message via ALL channels:");
        Console.WriteLine();

        List<NotificationCreator> allCreators = [emailCreator, smsCreator, pushCreator];
        foreach (var creator in allCreators)
        {
            creator.SendNotification("admin@company.com", "Server CPU usage exceeded 90%!");
            Console.WriteLine();
        }

        Console.WriteLine("-- Key Takeaway --");
        Console.WriteLine();
        Console.WriteLine("   The client code calls the SAME method (SendNotification) on");
        Console.WriteLine("   different creators. Each creator internally decides which");
        Console.WriteLine("   notification type to create. Adding a new channel (e.g., Slack)");
        Console.WriteLine("   requires ZERO changes to existing code - just add a new creator.");
        Console.WriteLine();
    }
}
