using AutoMapper;

namespace Backend.Configs
{
    public static class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(BackendLogic.Configs.Mapping.MapperProfile).Assembly));
            return configuration.CreateMapper();
        }
    }
}
