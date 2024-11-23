
namespace Common.Queue.Message
{
    public class MessageCollection : BaseMessage
    {
        public IEnumerable<BaseMessage>? BaseMessages { get; set; }
    }
}
