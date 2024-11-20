using Common.Enums;

namespace DAL.Enteties.ClientOperations.Request
{
    public class ClientWithdrawOperationRequest
    {
        public Guid client_id { get; set; }
        public ClientDepartmentAddressRequest? department_address { get; set; }
        public decimal amount { get; set; }
        public CurrencyISONames currency { get; set; }
    }
}
