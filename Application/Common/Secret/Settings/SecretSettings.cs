using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Secret.Settings
{
    public class SecretSettings
    {
        public QueueConnectionFactorySettings? ConnectionFactorySettings { get; set; }
        public DataBaseSettings? DataBaseSettings { get; set; }
    }
}
