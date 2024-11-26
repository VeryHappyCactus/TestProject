using Common.Settings.Json;

namespace Common.Settings
{
    public class AppCommonSettings : IAppCommonSettings
    {
        public IJsonSettings JsonSettings { get; init; } = new JsonSettings();
    
    }
}
