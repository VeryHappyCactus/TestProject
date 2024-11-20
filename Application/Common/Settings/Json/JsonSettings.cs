using System.Text.Json;

namespace Common.Settings.Json
{
    public class JsonSettings : IJsonSettings
    {
        public JsonSerializerOptions JsonSerializerOption { get; init; }

        public JsonSettings()
        {
            JsonSerializerOption = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
