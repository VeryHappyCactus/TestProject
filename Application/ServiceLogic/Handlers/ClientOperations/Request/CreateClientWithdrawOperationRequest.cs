using MediatR;
using Common.Enums;

using ServiceLogic.Handlers.ClientOperations.Result;

namespace ServiceLogic.Handlers.ClientOperations.Request
{
    public class CreateClientWithdrawOperationRequest : IRequest<CreateClientWithdrawOperationResult?>
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
        public decimal Amount { get; set; }
        public CurrencyISONames CurrencyISOName { get; set; }
    }
}
