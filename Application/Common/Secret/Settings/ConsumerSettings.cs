
namespace Common.Secret.Settings
{
    public class ConsumerSettings
    {
        public string? QueueName { get; set; }
        public string? ExchangeName { get; set; }
        public string? RoutingKey { get; set; }
        public bool IsAutoAck { get; set; } = true;
    }
}
