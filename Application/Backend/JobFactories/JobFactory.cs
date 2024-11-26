using Autofac;

namespace BackendLogic.JobFactories
{
    public class JobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public JobFactory(IContainer container)
        {
            if (container == null) 
                throw new ArgumentNullException(nameof(container));

            _container = container;
        }

        public IJob CreateJob(object message)
        {
            return _container.ResolveNamed<IJob>(message.GetType().Name);
        }
    }
}
