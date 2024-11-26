using Common.Settings.Json;

namespace Common.Settings
{
    public interface IAppCommonSettings
    {
        public JsonSettings JsonSettings { get; init; }
    }
}
