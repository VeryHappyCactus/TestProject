namespace Common.Queue.Settings
{
    public class QueueConnectionFactorySettings
    {
        public ConnectionSettings? ConsumerConncetionSettings { get; set; }
        public ConnectionSettings? ProducerConnectionSettings { get; set; }
        public ConsumerSettings? ConsumerSettings { get; set; }
        public ProducerSettings? ProducerSettings { get; set; }
    }
}
