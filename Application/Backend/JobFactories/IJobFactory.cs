using Backend.Jobs;

namespace Backend.JobFactories
{
    public interface IJobFactory
    {
        public IJob CreateJob(object message);
    }
}
