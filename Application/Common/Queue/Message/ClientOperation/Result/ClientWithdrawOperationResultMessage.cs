
namespace Common.Queue.Message.ClientOperation.Result
{
    public class ClientWithdrawOperationResultMessage : BaseMessage
    {
        public Guid ClientOperationId { get; set; }
    }
}
