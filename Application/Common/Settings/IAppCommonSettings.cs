using Common.Settings.Json;

namespace Common.Settings
{
    public interface IAppCommonSettings
    {
        public IJsonSettings JsonSettings { get; init; }
    }
}
