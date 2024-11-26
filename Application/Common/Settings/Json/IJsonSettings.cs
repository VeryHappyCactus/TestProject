using System.Text.Json;

namespace Common.Settings.Json
{
    public interface IJsonSettings
    {
        public JsonSerializerOptions JsonSerializerOption { get; init; }
    }
}
