using AutoMapper;

using MediatorHandlers.Managers;
using MediatorHandlers.Handlers.ClientOperations.Result;
using MediatorHandlers.Handlers.ClientOperations.Request;

using Common.Queue.Message.ClientOperation.Request;

namespace MediatorHandlers.Handlers.ClientOperations
{
    public class GetExchangeCourseHandler : BaseHandler<GetExchangeCourseRequest, IEnumerable<ExchangeCourseResult>?, ExchangeCourseRequestMessage>
    {
        public GetExchangeCourseHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper) { }
    }
}
