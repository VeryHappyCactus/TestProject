using Common.Queue;
using Common.Queue.Consumer;
using Common.Secret;
using Common.Settings;
using Common.Settings.Json;

using MediatorHandlers.Managers;

namespace Service.Configs
{
    public static class DIConfig
    {
        public static void RegisterTypes(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppCommonSettings, AppCommonSettings>();
            serviceCollection.AddSingleton<ISecretManager, SecretManager>();
            serviceCollection.AddSingleton<IJsonSettings, JsonSettings>();
            serviceCollection.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(sp => 
                new QueueConnectionFactory(sp.GetService<ISecretManager>()!, sp.GetService<IAppCommonSettings>()!, sp.GetService<ILogger<QueueConsumer>>()!));
            serviceCollection.AddSingleton<IQueueManager, QueueManager>();
        }
    }
}
