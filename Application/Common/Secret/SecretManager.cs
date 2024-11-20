using Common.Secret.Settings;
using Common.Settings;
using System.Text.Json;

namespace Common.Secret
{
    public class SecretManager : ISecretManager
    {
        private readonly string? _keySecretSettings;
        
        private readonly IAppCommonSettings _appCommonSettings;

        public SecretSettings SecretSettings { get; init; }

        public SecretManager(IAppCommonSettings appCommonSettings)
        {
            if (appCommonSettings == null)
                throw new ArgumentNullException(nameof(appCommonSettings));

            _appCommonSettings = appCommonSettings;
            _keySecretSettings = Environment.GetEnvironmentVariable("keySecretSettings");

            SecretSettings = GetSecretSettings();
        }

        public SecretSettings GetSecretSettings()
        {
            string json = File.ReadAllText(_keySecretSettings!);
            return JsonSerializer.Deserialize<SecretSettings>(json, _appCommonSettings.JsonSettings.JsonSerializerOption)!;
        }
    }
}
