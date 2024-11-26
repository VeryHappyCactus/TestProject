using Common.DB.Settings;
using Common.Queue.Settings;

namespace Common.Secret.Settings
{
    public class SecretSettings
    {
        public QueueConnectionFactorySettings? ConnectionFactorySettings { get; set; }
        public DataBaseSettings? DataBaseSettings { get; set; }
    }
}
