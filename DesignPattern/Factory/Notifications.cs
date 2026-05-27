namespace DesignPattern.Factory;

public class EmailNotification : INotification
{
    public string ChannelName => "Email";
    public TimeSpan EstimatedDeliveryTime => TimeSpan.FromSeconds(5);

    public void Send(string recipient, string message)
    {
        Console.WriteLine($"   {ChannelName} -> Connecting to SMTP server...");
        Console.WriteLine($"   {ChannelName} -> Composing HTML email template...");
        Console.WriteLine($"   {ChannelName} -> Sent to '{recipient}': \"{message}\"");
        Console.WriteLine($"   {ChannelName} -> Estimated delivery: {EstimatedDeliveryTime.TotalSeconds}s");
    }
}

public class SmsNotification : INotification
{
    private const int MaxSmsLength = 160;

    public string ChannelName => "SMS";
    public TimeSpan EstimatedDeliveryTime => TimeSpan.FromSeconds(2);

    public void Send(string recipient, string message)
    {
        var truncatedMessage = message.Length > MaxSmsLength
            ? message[..MaxSmsLength] + "..."
            : message;

        Console.WriteLine($"   {ChannelName} -> Connecting to carrier gateway...");
        Console.WriteLine($"   {ChannelName} -> Message length: {truncatedMessage.Length}/{MaxSmsLength} chars");
        Console.WriteLine($"   {ChannelName} -> Sent to '{recipient}': \"{truncatedMessage}\"");
        Console.WriteLine($"   {ChannelName} -> Estimated delivery: {EstimatedDeliveryTime.TotalSeconds}s");
    }
}

public class PushNotification : INotification
{
    public string ChannelName => "Push";
    public TimeSpan EstimatedDeliveryTime => TimeSpan.FromMilliseconds(500);

    public void Send(string recipient, string message)
    {
        Console.WriteLine($"   {ChannelName} -> Looking up device token for '{recipient}'...");
        Console.WriteLine($"   {ChannelName} -> Building push payload (title + body)...");
        Console.WriteLine($"   {ChannelName} -> Sent to '{recipient}': \"{message}\"");
        Console.WriteLine($"   {ChannelName} -> Estimated delivery: {EstimatedDeliveryTime.TotalMilliseconds}ms (instant)");
    }
}
