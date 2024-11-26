namespace Common.Queue.Settings
{
    public class ProducerSettings
    {
        public string? QueueName { get; set; }
        public string? ExchangeName { get; set; }
        public string? RoutingKey { get; set; }
    }
}
