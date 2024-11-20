
namespace Common.Queue.Message.ClientOperation.Request
{
    public class ClientOperationsRequestMessage : BaseMessage
    {
        public Guid ClientId { get; set; }
        public ClientDepartmentAddressMessage? DepartmentAddress { get; set; }
    }
}
