using MediatR;

using Common.Enums;

using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations.Request
{
    public class CreateClientWithdrawOperationRequest : IRequest<CreateClientWithdrawOperationResult?>
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
        public decimal Amount { get; set; }
        public CurrencyISONames CurrencyISOName { get; set; }
    }
}
