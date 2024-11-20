using Common.Queue.Message;

namespace Backend.Jobs
{
    public interface IJob
    {
        public Task<BaseMessage> ExecuteAsync<T>(T message)
            where T : BaseMessage;
    }
}
