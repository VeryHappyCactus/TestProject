using AutoMapper;

using Common.Queue.Message;

namespace MediatorHandlers.Configs.Mapping.Converters
{
    public class MessageCollectionConverter<TDestination> : ITypeConverter<MessageCollection, TDestination>
    {
        public TDestination Convert(MessageCollection source, TDestination destination, ResolutionContext context)
        {
            return context.Mapper.Map<TDestination>(source.BaseMessages);
        }
    }
}
