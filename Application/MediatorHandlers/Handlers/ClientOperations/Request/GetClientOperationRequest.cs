using MediatR;

using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations.Request
{
    public class GetClientOperationRequest : IRequest<HandlerResultContext<GetClientOperationResult>>
    {
        public Guid ClientOperationId { get; set; }
    }
}
