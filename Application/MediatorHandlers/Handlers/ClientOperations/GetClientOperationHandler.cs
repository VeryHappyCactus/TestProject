using AutoMapper;

using Common.Queue.Message.ClientOperation.Request;

using MediatorHandlers.Managers;
using MediatorHandlers.Handlers.ClientOperations.Request;
using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations
{
    public class GetClientOperationHandler : BaseHandler<GetClientOperationRequest, GetClientOperationResult?, ClientOperationRequestMessage>
    {
        public GetClientOperationHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper) { }
    }
}
