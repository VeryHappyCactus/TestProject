using System.Text.Json;

namespace Common.Settings.Json
{
    public interface IJsonSettings
    {
        public JsonSerializerOptions JsonSerializerOptions { get; init; }
    }
}
