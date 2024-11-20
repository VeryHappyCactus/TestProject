using Common.Queue.Message;

namespace Common.Queue.Consumer.Delegates
{
    public delegate Task AsyncMessageEventHadler(MessageContext context);
}
