using AutoMapper;

namespace Backend.Configs
{
    public static class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Program).Assembly));
            return configuration.CreateMapper();
        }
    }
}
