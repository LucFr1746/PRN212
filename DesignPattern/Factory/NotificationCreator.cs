namespace DesignPattern.Factory;

public abstract class NotificationCreator
{
    protected abstract INotification CreateNotification();

    public void SendNotification(string recipient, string message)
    {
        INotification notification = CreateNotification();

        Console.WriteLine($"   Validating recipient '{recipient}' for {notification.ChannelName}...");

        if (string.IsNullOrWhiteSpace(recipient))
        {
            Console.WriteLine("   Recipient cannot be empty!");
            return;
        }

        notification.Send(recipient, message);

        Console.WriteLine($"   Notification queued via {notification.ChannelName}");
    }
}

public class EmailNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new EmailNotification();
}

public class SmsNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new SmsNotification();
}

public class PushNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new PushNotification();
}
