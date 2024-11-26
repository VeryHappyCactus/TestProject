using Common.Settings.Json;

namespace Common.Settings
{
    public class AppCommonSettings : IAppCommonSettings
    {
        public JsonSettings JsonSettings { get; init; } = new JsonSettings();
    
    }
}
