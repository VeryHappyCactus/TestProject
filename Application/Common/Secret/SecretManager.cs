using System.Text.Json;

using Common.Secret.Settings;
using Common.Settings;

namespace Common.Secret
{
    public class SecretManager : ISecretManager
    {
        private readonly string? _keySecretSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SecretSettings SecretSettings { get; init; }

        public SecretManager(JsonSerializerOptions jsonSerializerOptions)
        {
            if (jsonSerializerOptions == null)
                throw new ArgumentNullException(nameof(jsonSerializerOptions));

            _jsonSerializerOptions = jsonSerializerOptions;
            _keySecretSettings = Environment.GetEnvironmentVariable("keySecretSettings");

            SecretSettings = GetSecretSettings();
        }

        public SecretSettings GetSecretSettings()
        {
            string json = File.ReadAllText(_keySecretSettings!);
            return JsonSerializer.Deserialize<SecretSettings>(json, _jsonSerializerOptions)!;
        }
    }
}
