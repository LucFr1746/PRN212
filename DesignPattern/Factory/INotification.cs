namespace DesignPattern.Factory;

public interface INotification
{
    void Send(string recipient, string message);
    string ChannelName { get; }
    TimeSpan EstimatedDeliveryTime { get; }
}
