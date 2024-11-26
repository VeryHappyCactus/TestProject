namespace BackendLogic.JobFactories
{
    public interface IJobFactory
    {
        public IJob CreateJob(object message);
    }
}
