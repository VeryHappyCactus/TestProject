
namespace Common.Queue.Message
{
    public class ErrorMessage : BaseMessage
    {
        public string? MessageType { get; set; }
        public int ErrorCode { get; set; }
        
    }
}
