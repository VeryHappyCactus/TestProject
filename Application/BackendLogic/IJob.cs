using Common.Queue.Message;

namespace BackendLogic
{
    public interface IJob
    {
        public Task<BaseMessage> ExecuteAsync<T>(T message)
            where T : BaseMessage;
    }
}
