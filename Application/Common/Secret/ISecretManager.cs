using Common.Secret.Settings;

namespace Common.Secret
{
    public interface ISecretManager
    {
        public SecretSettings SecretSettings { get; init; }
    }
}
