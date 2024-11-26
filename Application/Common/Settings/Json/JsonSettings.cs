using System.Text.Json;

namespace Common.Settings.Json
{
    public class JsonSettings : IJsonSettings
    {
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        public JsonSettings()
        {
            JsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
