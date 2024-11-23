using MediatR;

using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations.Request
{
    public class GetClientOperationsRequest : IRequest<IEnumerable<GetClientOperationResult>?>
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
    }
}
