using AutoMapper;

using MediatorHandlers.Handlers;

namespace Service.Configs
{
    public static class ServicesConfig
    {
        public static void RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMapper>(GetMapper());
            serviceCollection.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(HandlerResultContext<>).Assembly);
            });
        }

        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Service.Configs.Mapping.MapperProfile).Assembly);
                cfg.AddMaps(typeof(MediatorHandlers.Configs.Mapping.MapperProfile).Assembly);
            });
            return configuration.CreateMapper();
        }
    }
}
