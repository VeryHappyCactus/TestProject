using Common.Enums;

namespace Common.Queue.Message.ClientOperation.Request
{
    public class ClientWithdrawOperationRequestMessage : BaseMessage
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressMessage? DepartmentAddress { get; set; }
        public decimal Amount { get; set; }
        public CurrencyISONames CurrencyISOName { get; set; }
    }
}
