using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Settings.Json;

namespace Common.Settings
{
    public class AppCommonSettings : IAppCommonSettings
    {
        public JsonSettings JsonSettings { get; init; } = new JsonSettings();
    
    }
}
