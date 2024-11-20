
namespace Common.Queue.Message
{
    public class MessageContext
    {
        public string? PublisherId { get; set; }
        public string? SessionId { get; set; }
        public string? RequestId { get; set; }
        public BaseMessage? Body {  get; set; }
    }
}
