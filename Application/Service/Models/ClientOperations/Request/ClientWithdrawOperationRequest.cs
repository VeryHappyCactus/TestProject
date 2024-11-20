using Common.Enums;

namespace Service.Models.ClientOperations.Request
{
    public class ClientWithdrawOperationRequest
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressRequest? DepartmentAddress { get; set; }
        public decimal Amount { get; set; }
        public CurrencyISONames CurrencyISOName { get; set; }
    }
}
