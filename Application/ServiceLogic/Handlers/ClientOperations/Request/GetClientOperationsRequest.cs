using MediatR;

using ServiceLogic.Handlers.ClientOperations.Result;

namespace ServiceLogic.Handlers.ClientOperations.Request
{
    public class GetClientOperationsRequest : IRequest<IEnumerable<GetClientOperationResult>?>
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
    }
}
