using Microsoft.AspNetCore.Diagnostics;

using Common.Queue;
using Common.Queue.Consumer;
using Common.Secret;
using Common.Settings;
using Common.Settings.Json;

using Service.Handlers;
using ServiceLogic.Managers;

namespace Service.Configs
{
    public static class DIConfig
    {
        public static void RegisterTypes(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppCommonSettings, AppCommonSettings>();
            serviceCollection.AddSingleton<ISecretManager, SecretManager>(sp => 
                new SecretManager(sp.GetService<IAppCommonSettings>()!.JsonSettings!.JsonSerializerOptions!));
            
            serviceCollection.AddSingleton<IQueueManager, QueueManager>();
            serviceCollection.AddSingleton<IExceptionHandler, GlobalErrorHandler>(sp => new GlobalErrorHandler(
                sp.GetService<IAppCommonSettings>()!.JsonSettings!.JsonSerializerOptions!,
                sp.GetService<ILogger<GlobalErrorHandler>>()!
            ));
            serviceCollection.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(sp => new QueueConnectionFactory
            (
                sp.GetService<ISecretManager>()!.SecretSettings!.ConnectionFactorySettings!, 
                sp.GetService<IAppCommonSettings>()!.JsonSettings.JsonSerializerOptions, 
                sp.GetService<ILogger<QueueConsumer>>()!
            ));
        }
    }
}
