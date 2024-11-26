using MediatR;

using ServiceLogic.Handlers.ClientOperations.Result;

namespace ServiceLogic.Handlers.ClientOperations.Request
{
    public class GetClientOperationRequest : IRequest<GetClientOperationResult?>
    {
        public Guid ClientOperationId { get; set; }
    }
}
