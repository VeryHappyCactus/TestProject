using Autofac;
using AutoMapper;
using NLog.Extensions.Logging;

using Common.Queue;
using Common.Settings;
using Common.Settings.Json;
using Common.Queue.Message.ClientOperation.Request;
using Common.Secret;

using DAL;
using DAL.Contexts;
using DAL.Respositories;

using Backend.Queue;
using Backend.Jobs;
using Backend.JobFactories;
using Common.Queue.Consumer;

namespace Backend.Configs
{
    public static class AutofacConfig
    {
        public static IContainer GetDIContainer()
        {
            NLogLoggerFactory loggerFactory = new NLogLoggerFactory();
            ContainerBuilder containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<AppCommonSettings>()
                .As<IAppCommonSettings>()
                .SingleInstance();

            containerBuilder.RegisterType<SecretManager>()
                .As<ISecretManager>()
                .SingleInstance();

            containerBuilder.RegisterType<DataContext>()
                .As<IDataContext>()
                .SingleInstance();

            containerBuilder.RegisterType<ClientOperationRepository>()
                .As<IClientOperationRepository>()
                .SingleInstance();

            containerBuilder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();

            containerBuilder.RegisterType<QueueConnectionFactory>()
                .As<IQueueConnectionFactory>()
                .WithParameter("logger", loggerFactory.CreateLogger(typeof(QueueConsumer).Name))
                .SingleInstance();

            containerBuilder.RegisterType<ApplicationManager>()
                .As<IApplicationManager>()
                .WithParameter("logger", loggerFactory.CreateLogger(typeof(Program).Name))
                .SingleInstance();

            containerBuilder.RegisterInstance(AutoMapperConfig.GetMapper())
                .As<IMapper>()
                .SingleInstance();

            containerBuilder.RegisterType<JobFactory>()
                .As<IJobFactory>()
                .SingleInstance();

            containerBuilder.RegisterType<ClientWithdrawOperationJob>()
                .Named<IJob>(nameof(ClientWithdrawOperationRequestMessage))
                .SingleInstance();

            containerBuilder.RegisterType<ClientOperationByIdJob>()
                .Named<IJob>(nameof(ClientOperationRequestMessage))
                .SingleInstance();

            containerBuilder.RegisterType<ClientOperationsByClientJob>()
                .Named<IJob>(nameof(ClientOperationsRequestMessage))
                .SingleInstance();

            containerBuilder.RegisterType<JsonSettings>()
                .As<JsonSettings>()
                .SingleInstance();

            containerBuilder.RegisterType<AppCommonSettings>()
                .As<IAppCommonSettings>()
                .SingleInstance();

            IContainer container = containerBuilder.Build();
            container.Resolve<IJobFactory>(new NamedParameter("container", container));

            return container;
        }

    }
}
